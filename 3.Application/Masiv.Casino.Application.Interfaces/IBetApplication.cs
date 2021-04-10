using Masiv.Casino.Application.Contracts.DTO;
using Masiv.Casino.Domain.Entities;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Interfaces
{
    public interface IBetApplication
    {
        Task<GenericResponse> Create(BetDto betDTO, string userId);
    }
}