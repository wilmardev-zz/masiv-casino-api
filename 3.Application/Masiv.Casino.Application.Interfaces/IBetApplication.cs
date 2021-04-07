using Masiv.Casino.Domain.Entities;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Interfaces
{
    public interface IBetApplication
    {
        Task<GenericResponse> Create(Bet bet);
    }
}