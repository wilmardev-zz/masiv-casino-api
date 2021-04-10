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
        private readonly ICacheRepository cacheRepository;
        private readonly AppSettings appSettings;

        public BetService(ICacheRepository cacheRepository, IOptions<AppSettings> appSettings)
        {
            this.cacheRepository = cacheRepository;
            this.appSettings = appSettings.Value;
        }

        public async Task<GenericResponse> Create(Bet bet)
        {
            List<Bet> betList = await Get();
            List<Roulette> rouletteList = await cacheRepository.Get<Roulette>(appSettings.RouletteCacheKey);
            Roulette selectedRoulette = rouletteList.FirstOrDefault(x => x.Id == bet.RouletteId);
            GenericResponse validation = ValidateStatusRoulette(selectedRoulette);
            if (!validation.Success)
                return validation;
            betList.Add(bet);
            await cacheRepository.Save(betList, appSettings.BetCacheKey);
            return Helper.ManageResponse();
        }

        public async Task<List<Bet>> Get()
        {
            return await cacheRepository.Get<Bet>(appSettings.BetCacheKey);
        }

        private GenericResponse ValidateStatusRoulette(Roulette selectedRoulette)
        {
            ErrorResponse error = null;
            if (selectedRoulette == null)
                error = new ErrorResponse("ROULETE_NOT_FOUND", "The selected roulette not exist.");
            else if (!selectedRoulette.State.Equals(RouletteStatus.Open.ToString()))
                error = new ErrorResponse("ROULETE_NOT_OPEN", "The selected roulette is not open to play.");
            return Helper.ManageResponse(error, error == null);
        }
    }
}