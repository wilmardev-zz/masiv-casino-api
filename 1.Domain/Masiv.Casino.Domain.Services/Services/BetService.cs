using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Domain.Entities.Enums;
using Masiv.Casino.Domain.Interfaces.Repositories;
using Masiv.Casino.Domain.Interfaces.Services;
using Masiv.Casino.Domain.Services.Utilities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Services.Services
{
    public class BetService : IBetService
    {
        private readonly IBetRepository betRepository;

        public BetService(IBetRepository betRepository)
        {
            this.betRepository = betRepository;
        }

        public async Task<GenericResponse> Create(Bet bet)
        {
            int response = await this.betRepository.Save(bet);
            GenericResponse validation = ValidateStatusRoulette(response);
            if (!validation.Success)
                return validation;
            return Helper.ManageResponse();
        }

        private GenericResponse ValidateStatusRoulette(int dbResult)
        {
            ErrorResponse error = null;
            if (dbResult == 3)
                error = new ErrorResponse(Constants.ROULETTE_NOT_FOUND, Constants.ROULETTE_NOT_FOUND_DESC);
            else if (dbResult == 2)
                error = new ErrorResponse(Constants.ROULETTE_NOT_OPEN, Constants.ROULETTE_NOT_OPEN_DESC);
            return Helper.ManageResponse(error, error == null);
        }
    }
}