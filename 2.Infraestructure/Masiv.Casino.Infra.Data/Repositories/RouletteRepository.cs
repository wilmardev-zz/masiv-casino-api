using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Interfaces.GenericRepository;
using Masiv.Casino.Domain.Interfaces.Repositories;

namespace Masiv.Casino.Infra.Data.Repositories
{
    public class RouletteRepository : IRouletteRepository
    {
        private readonly IDbFactory dbFactory;

        public RouletteRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<List<Roulette>> Get()
        {
            using var db = await this.dbFactory.GetConnection();
            var roulettes = await db.QueryAsync<Roulette>("[Casino].[GetRoulette]",
                commandType: System.Data.CommandType.StoredProcedure);
            return roulettes.ToList();
        }

        public async Task Save<T>(Roulette roulette)
        {
            var parameters = new DynamicParameters();
            parameters.Add("RouletteId", roulette.Id, DbType.String);
            using var db = await this.dbFactory.GetConnection();
            var query = await db.QueryAsync("[Casino].[SaveRoulette]",
                commandType: CommandType.StoredProcedure,
                param: parameters);
            var response = query.FirstOrDefault();
        }

        public async Task<int> Open(string rouletteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("RouletteId", rouletteId);
            parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using var db = await this.dbFactory.GetConnection();
            var query = await db.QueryAsync("[Casino].[OpenRoulette]",
                commandType: CommandType.StoredProcedure,
                param: parameters);
            var response = parameters.Get<int>("@Result");
            return response;
        }

        public async Task<int> Close(string rouletteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("RouletteId", rouletteId);
            parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using var db = await this.dbFactory.GetConnection();
            var query = await db.QueryAsync("[Casino].[CloseRoulette]",
                commandType: CommandType.StoredProcedure,
                param: parameters);
            var response = parameters.Get<int>("@Result");
            return response;
        }
    }
}
