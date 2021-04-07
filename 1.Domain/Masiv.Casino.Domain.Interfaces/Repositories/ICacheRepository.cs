using Masiv.Casino.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Interfaces.Repositories
{
    public interface ICacheRepository
    {
        Task<Roulette> Get(string rouletteId);

        Task Save(List<Roulette> roulettes);
    }
}