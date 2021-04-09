using Masiv.Casino.Domain.Interfaces.Repositories;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Masiv.Casino.Infra.Data.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase db;

        public CacheRepository()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            db = redis.GetDatabase(0);
        }

        public async Task<List<T>> Get<T>(string cacheKey)
        {
            return await Task.Run(() =>
            {
                var cacheData = db.StringGet(cacheKey);
                if (cacheData.IsNullOrEmpty)
                    return new List<T>();
                return JsonSerializer.Deserialize<List<T>>(cacheData);
            });
        }

        public async Task Save<T>(List<T> entity, string cacheKey)
        {
            await Task.Run(() =>
            {
                var data = JsonSerializer.Serialize(entity).ToString();
                db.StringSet(cacheKey, data);
            });
        }
    }
}