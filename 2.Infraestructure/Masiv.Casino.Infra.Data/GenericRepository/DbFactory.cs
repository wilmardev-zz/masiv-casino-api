using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Domain.Interfaces.GenericRepository;
using Microsoft.Extensions.Options;

namespace Masiv.Casino.Infra.Data.GenericRepository
{
    public class DbFactory : IDbFactory
    {
        private readonly string connectionString;

        public DbFactory(IOptions<AppSettings> appSettings)
        {
            this.connectionString = appSettings.Value.ConnectionString;
        }

        public async Task<IDbConnection> GetConnection()
        {
            var connection = new SqlConnection(this.connectionString);
            await connection.OpenAsync().ConfigureAwait(false);
            return connection;
        }
    }
}
