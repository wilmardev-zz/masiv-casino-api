using Masiv.Casino.Domain.Interfaces.Repositories;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Masiv.Casino.Infra.Data.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IRedisDatabase redisDatabase;

        public CacheRepository(IRedisCacheClient redisCacheClient)
        {
            this.redisDatabase = redisCacheClient.GetDbFromConfiguration();
        }

        public async Task<List<T>> Get<T>(string cacheKey)
        {
            var cacheData = await redisDatabase.GetAsync<object>(cacheKey);
            if (cacheData == null)
                return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(cacheData.ToString());
        }

        public async Task Save<T>(List<T> entity, string cacheKey)
        {
            string data = JsonSerializer.Serialize(entity).ToString();
            await redisDatabase.AddAsync(cacheKey, data);
        }
    }
}