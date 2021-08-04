using System;
using System.Data;
using System.Threading.Tasks;

namespace Masiv.Casino.Domain.Interfaces.GenericRepository
{
    public interface IDbFactory
    {
        Task<IDbConnection> GetConnection();
    }
}
