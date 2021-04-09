using Masiv.Casino.Domain.Entities;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Interfaces
{
    public interface IRouletteApplication
    {
        Task<GenericResponse> Create();

        Task<GenericResponse> Open(Roulette roulette);

        Task<GenericResponse> Close(Roulette roulette);

        Task<GenericResponse> Get();
    }
}