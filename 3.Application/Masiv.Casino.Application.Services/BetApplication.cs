using Masiv.Casino.Application.Contracts.DTO;
using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Interfaces.Services;
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

        public async Task<GenericResponse> Create(BetDto betDTO, string userId)
        {
            Bet bet = new Bet
            {
                RouletteId = betDTO.RouletteId,
                Quantity = betDTO.Quantity,
                Color = betDTO.Color,
                Number = betDTO.Number,
                UserId = userId
            };
            return await betService.Create(bet);
        }
    }
}