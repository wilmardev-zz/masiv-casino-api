using System.Threading.Tasks;
using Dapper;
using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Interfaces.GenericRepository;
using Masiv.Casino.Domain.Interfaces.Repositories;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace Masiv.Casino.Infra.Data.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly IDbFactory dbFactory;

        public BetRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<List<Bet>> GetByRouletteId(string rouletteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("RouletteId", rouletteId);
            using var db = await this.dbFactory.GetConnection();
            var query = await db.QueryAsync<Bet>("[Casino].[GetRouletteBetById]",
                commandType: System.Data.CommandType.StoredProcedure,
                param: parameters);
            var response = query.ToList();
            return response;
        }

        public async Task<int> Save(Bet bet)
        {
            var parameters = new DynamicParameters();
            parameters.Add("RouletteId", bet.RouletteId);
            parameters.Add("UserId", bet.UserId);
            parameters.Add("Number", bet.Number);
            parameters.Add("Color", bet.Color);
            parameters.Add("Quantity", bet.Quantity);
            parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using var db = await this.dbFactory.GetConnection();
            var query = await db.QueryAsync("[Casino].[SaveBet]",
                commandType: System.Data.CommandType.StoredProcedure,
                param: parameters);
            var response = parameters.Get<int>("@Result");
            return response;

        }

        public async Task Update<T>(Bet bet)
        {
            var parameters = new DynamicParameters();
            parameters.Add("RouletteId", bet.RouletteId);
            parameters.Add("BetId", bet.Id);
            parameters.Add("TotalWinner", bet.TotalBetWinner);
            using var db = await this.dbFactory.GetConnection();
            var query = await db.QueryAsync("[Casino].[UpdateBet]",
                commandType: System.Data.CommandType.StoredProcedure,
                param: parameters);
            var response = query.FirstOrDefault();

        }
    }
}
