    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Masiv.Casino.Domain.Entities;

    namespace Masiv.Casino.Domain.Interfaces.Repositories
    {
        public interface IBetRepository
        {

            Task<List<Bet>> GetByRouletteId(string rouletteId);

            Task<int> Save(Bet bet);

            Task Update<T>(Bet bet);
        }
    }
