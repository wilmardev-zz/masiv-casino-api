using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Infra.Data.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        public Task<Roulette> Get(string rouletteId)
        {
            throw new System.NotImplementedException();
        }

        public Task Save(List<Roulette> roulettes)
        {
            throw new System.NotImplementedException();
        }
    }
}