using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Masiv.Casino.Domain.Entities;

namespace Masiv.Casino.Domain.Interfaces.Repositories
{
    public interface IRouletteRepository
    {
        Task<List<Roulette>> Get();

        Task Save<T>(Roulette roulette);

        Task<int> Open(string rouletteId);

        Task<int> Close(string rouletteId);
    }
}
