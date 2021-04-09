using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Interfaces.Repositories
{
    public interface ICacheRepository
    {
        Task<List<T>> Get<T>(string cacheKey);

        Task Save<T>(List<T> entity, string cacheKey);
    }
}