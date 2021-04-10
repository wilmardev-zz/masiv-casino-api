using Masiv.Casino.Domain.Entities;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Interfaces
{
    public interface IRouletteApplication
    {
        Task<GenericResponse> Create();

        Task<GenericResponse> Open(string rouletteId);

        Task<GenericResponse> Close(string rouletteId);

        Task<GenericResponse> Get();
    }
}