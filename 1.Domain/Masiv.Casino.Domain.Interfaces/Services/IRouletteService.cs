using Masiv.Casino.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Interfaces.Services
{
    public interface IRouletteService
    {
        Task<string> Create();

        Task<GenericResponse> Open(Roulette roulette);

        Task<GenericResponse> Close(Roulette roulette);

        Task<List<Roulette>> Get();
    }
}