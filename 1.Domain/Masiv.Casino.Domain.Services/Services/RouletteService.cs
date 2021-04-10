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
        private readonly ICacheRepository cacheRepository;
        private readonly AppSettings appSettings;
        private readonly Random random = new Random();

        public RouletteService(ICacheRepository cacheRepository, IOptions<AppSettings> appSettings)
        {
            this.cacheRepository = cacheRepository;
            this.appSettings = appSettings.Value;
        }

        public async Task<GenericResponse> Close(string rouletteId)
        {
            List<Bet> betList = await cacheRepository.Get<Bet>(appSettings.BetCacheKey);
            List<Bet> betListSelected = betList.FindAll(x => x.RouletteId.Equals(rouletteId));
            GenericResponse resultChangeStatus = await ChangeRouletteStatus(rouletteId, false);
            BetResult betResult = SelectBetWinner(betListSelected, rouletteId);
            if (!resultChangeStatus.Success)
                return resultChangeStatus;
            return Helper.ManageResponse(betResult);
        }

        public async Task<string> Create()
        {
            List<Roulette> rouletteList = await Get();
            Roulette roulette = new Roulette();
            rouletteList.Add(roulette);
            await cacheRepository.Save(rouletteList, appSettings.RouletteCacheKey);
            return roulette.Id;
        }

        public async Task<List<Roulette>> Get()
        {
            return await cacheRepository.Get<Roulette>(appSettings.RouletteCacheKey);
        }

        public async Task<GenericResponse> Open(string rouletteId)
        {
            return await ChangeRouletteStatus(rouletteId, true);
        }

        private GenericResponse ValidateRouletteStatus(Roulette roulette, bool isOpen)
        {
            ErrorResponse error = null;
            if (roulette == null)
                error = new ErrorResponse(Constants.ROULETTE_NOT_FOUND, Constants.ROULETTE_NOT_FOUND_DESC);
            else if (roulette.State.Equals(RouletteStatus.Close.ToString()))
                error = new ErrorResponse(Constants.ROULETTE_IS_CLOSED, Constants.ROULETTE_IS_CLOSED_DESC);
            else if (isOpen && roulette.State.Equals(RouletteStatus.Open.ToString()))
                error = new ErrorResponse(Constants.ROULETTE_ALREADY_OPEN, Constants.ROULETTE_ALREADY_OPEN_DESC);
            return Helper.ManageResponse(error, error == null);
        }

        private async Task<GenericResponse> ChangeRouletteStatus(string rouletteId, bool isOpen)
        {
            List<Roulette> rouletteList = await Get();
            Roulette selectedRoulette = rouletteList.FirstOrDefault(x => x.Id.Equals(rouletteId));
            GenericResponse validation = ValidateRouletteStatus(selectedRoulette, isOpen);
            if (!validation.Success)
                return validation;
            selectedRoulette.State = isOpen ? RouletteStatus.Open.ToString() : RouletteStatus.Close.ToString();
            selectedRoulette.CloseDate = !isOpen ? DateTime.UtcNow : (DateTime?)null;
            await cacheRepository.Save(rouletteList, appSettings.RouletteCacheKey);
            return Helper.ManageResponse();
        }

        private BetResult SelectBetWinner(List<Bet> betListSelected, string rouletteId)
        {
            int winnerNumber = random.Next(appSettings.MinBetValidNumber, appSettings.MaxBetValidNumber);
            bool isEvenNumber = winnerNumber % 2 == 0;
            decimal totalBet = 0;
            foreach (var bet in betListSelected)
            {
                CalculateMoneyWinner(bet, winnerNumber, isEvenNumber);
                totalBet += bet.TotalBetWinner;
            }
            return new BetResult
            {
                RouletteId = rouletteId,
                Bets = betListSelected,
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