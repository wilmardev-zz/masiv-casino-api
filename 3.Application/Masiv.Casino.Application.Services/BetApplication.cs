using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Interfaces.Services;
using Masiv.Casino.Domain.Services.Utilities;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Services
{
    public class BetApplication : IBetApplication
    {
        private readonly IBetService betService;

        public BetApplication(IBetService betService)
        {
            this.betService = betService;
        }

        public async Task<GenericResponse> Create(Bet bet)
        {
            var betId = await betService.Create(bet);
            return Helper.ManageResponse(betId);
        }
    }
}