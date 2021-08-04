using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Domain.Entities.Enums;
using Masiv.Casino.Domain.Interfaces.Repositories;
using Masiv.Casino.Domain.Interfaces.Services;
using Masiv.Casino.Domain.Services.Utilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Services.Services
{
    public class RouletteService : IRouletteService
    {
        private readonly IRouletteRepository rouletteRepository;
        private readonly IBetRepository betRepository;
        private readonly AppSettings appSettings;
        private readonly Random random = new Random();

        public RouletteService(IRouletteRepository rouletteRepository,
            IBetRepository betRepository , IOptions<AppSettings> appSettings)
        {
            this.rouletteRepository = rouletteRepository;
            this.betRepository = betRepository;
            this.appSettings = appSettings.Value;
        }

        public async Task<GenericResponse> Close(string rouletteId)
        {
            var response = await rouletteRepository.Close(rouletteId);
            var betList = await betRepository.GetByRouletteId(rouletteId);
            GenericResponse validation = ValidateRouletteStatus(response);
            if (!validation.Success)
                return validation;
            BetResult betResult = SelectBetWinner(betList, rouletteId);
            return Helper.ManageResponse(betResult);
        }

        public async Task<string> Create()
        {
            Roulette roulette = new Roulette();
            await rouletteRepository.Save<Roulette>(roulette);
            return roulette.Id;
        }

        public async Task<List<Roulette>> Get()
        {
            return await rouletteRepository.Get();
        }

        public async Task<GenericResponse> Open(string rouletteId)
        {
            var response = await rouletteRepository.Open(rouletteId);
            GenericResponse validation = ValidateRouletteStatus(response);
            if (!validation.Success)
                return validation;
            return Helper.ManageResponse();
        }

        private GenericResponse ValidateRouletteStatus(int dbResponse)
        {
            ErrorResponse error = null;
            if (dbResponse == 4)
                throw new BadRequest(Constants.ROULETTE_NOT_FOUND, Constants.ROULETTE_NOT_FOUND_DESC);
            else if (dbResponse == 3)
                throw new BadRequest(Constants.ROULETTE_IS_CLOSED, Constants.ROULETTE_IS_CLOSED_DESC);
            else if (dbResponse == 2)
                throw new BadRequest(Constants.ROULETTE_ALREADY_OPEN, Constants.ROULETTE_ALREADY_OPEN_DESC);
            return Helper.ManageResponse(error, error == null);
        }

        private BetResult SelectBetWinner(List<Bet> betList, string rouletteId)
        {
            int winnerNumber = random.Next(appSettings.MinBetValidNumber, appSettings.MaxBetValidNumber);
            bool isEvenNumber = winnerNumber % 2 == 0;
            decimal totalBet = 0;
            foreach (var bet in betList)
            {
                CalculateMoneyWinner(bet, winnerNumber, isEvenNumber);
                totalBet += bet.TotalBetWinner;
                betRepository.Update<Bet>(bet);

            }
            return new BetResult
            {
                RouletteId = rouletteId,
                Bets = betList,
                TotalBetWinners = totalBet,
                WinnerNumber = winnerNumber,
                WinnerColor = isEvenNumber ? BetColor.Red.ToString() : BetColor.Black.ToString(),
                ClosedDate = DateTime.UtcNow,
                HasWinner = totalBet != 0
            };
        }

        private void CalculateMoneyWinner(Bet bet, int winnerNumber, bool isEvenNumber)
        {
            if (bet.Number != null && bet.Number == winnerNumber)
                bet.TotalBetWinner = bet.Quantity * appSettings.BetNumericFee;
            else if (!string.IsNullOrEmpty(bet.Color) && ((bet.Color.ToUpper().Equals(BetColor.Red.ToString().ToUpper()) && isEvenNumber) ||
                (bet.Color.ToUpper().Equals(BetColor.Black.ToString().ToUpper()) && !isEvenNumber)))
                bet.TotalBetWinner = bet.Quantity * appSettings.BetColorFee;
        }
    }
}