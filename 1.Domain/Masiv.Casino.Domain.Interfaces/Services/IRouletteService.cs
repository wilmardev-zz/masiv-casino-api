using Masiv.Casino.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Interfaces.Services
{
    public interface IRouletteService
    {
        Task<string> Create();

        Task<GenericResponse> Open(string rouletteId);

        Task<GenericResponse> Close(string rouletteId);

        Task<List<Roulette>> Get();
    }
}