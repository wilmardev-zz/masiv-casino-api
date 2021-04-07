using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Services.Services
{
    public class RouletteService : IRouletteService
    {
        public Task<List<Bet>> Close(Roulette roulette)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Create()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Roulette>> Get()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Open(Roulette roulette)
        {
            throw new System.NotImplementedException();
        }
    }
}