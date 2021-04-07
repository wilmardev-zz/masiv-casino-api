using Masiv.Casino.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Interfaces.Services
{
    public interface IRouletteService
    {
        Task<string> Create();

        Task<bool> Open(Roulette roulette);

        Task<List<Bet>> Close(Roulette roulette);

        Task<List<Roulette>> Get();
    }
}