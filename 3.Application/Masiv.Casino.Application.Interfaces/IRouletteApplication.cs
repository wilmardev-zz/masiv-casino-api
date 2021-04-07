using Masiv.Casino.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Interfaces
{
    public interface IRouletteApplication
    {
        Task<GenericResponse> Create();

        Task<GenericResponse> Open(Roulette roulette);

        Task<List<GenericResponse>> Close(Roulette roulette);

        Task<List<GenericResponse>> Get();
    }
}