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
    public class RouletteService : IRouletteService
    {
        private readonly ICacheRepository cacheRepository;
        private readonly AppSettings appSettings;

        public RouletteService(ICacheRepository cacheRepository, IOptions<AppSettings> appSettings)
        {
            this.cacheRepository = cacheRepository;
            this.appSettings = appSettings.Value;
        }

        public async Task<GenericResponse> Close(Roulette roulette)
        {
            var betList = await cacheRepository.Get<Bet>(appSettings.BetCacheKey);
            var betListSelected = betList.FirstOrDefault(x => x.Id == roulette.Id);
            var resultChangeStatus = await ChangeRouletteStatus(roulette, false);
            if (!resultChangeStatus.Success)
                return resultChangeStatus;
            return Helper.ManageResponse(betListSelected);
        }

        public async Task<string> Create()
        {
            var rouletteList = await Get();
            var roulette = new Roulette();
            rouletteList.Add(roulette);
            await cacheRepository.Save(rouletteList, appSettings.RouletteCacheKey);
            return roulette.Id;
        }

        public async Task<List<Roulette>> Get()
        {
            return await cacheRepository.Get<Roulette>(appSettings.RouletteCacheKey);
        }

        public async Task<GenericResponse> Open(Roulette roulette)
        {
            return await ChangeRouletteStatus(roulette, true);
        }

        private GenericResponse ValidateRouletteStatus(Roulette roulette, bool isOpen)
        {
            ErrorResponse error = null;
            if (roulette == null)
                error = new ErrorResponse("ROULETE_NOT_FOUND", "The selected roulette not exist.");
            else if (roulette.State.Equals(RouletteStatus.Close.ToString()))
                error = new ErrorResponse("ROULETE_IS_CLOSED", "The selected roulette has been closed.");
            else if (isOpen && roulette.State.Equals(RouletteStatus.Open.ToString()))
                error = new ErrorResponse("ROULETE_ALREADY_OPEN", "The selected roulette is already open.");
            return Helper.ManageResponse(error, error == null);
        }

        private async Task<GenericResponse> ChangeRouletteStatus(Roulette roulette, bool isOpen)
        {
            var rouletteList = await Get();
            var selectedRoulette = rouletteList.FirstOrDefault(x => x.Id == roulette.Id);
            var validation = ValidateRouletteStatus(selectedRoulette, isOpen);
            if (!validation.Success)
                return validation;
            selectedRoulette.State = isOpen ? RouletteStatus.Open.ToString() : RouletteStatus.Close.ToString();
            await cacheRepository.Save(rouletteList, appSettings.RouletteCacheKey);
            return Helper.ManageResponse();
        }
    }
}