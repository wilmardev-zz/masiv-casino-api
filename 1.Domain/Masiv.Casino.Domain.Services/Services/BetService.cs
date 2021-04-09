using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Domain.Interfaces.Repositories;
using Masiv.Casino.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
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

        public async Task<string> Create(Bet bet)
        {
            var betList = await Get();
            betList.Add(bet);
            await cacheRepository.Save(betList, appSettings.BetCacheKey);
            return bet.Id;
        }

        public async Task<List<Bet>> Get()
        {
            return await cacheRepository.Get<Bet>(appSettings.BetCacheKey);
        }
    }
}