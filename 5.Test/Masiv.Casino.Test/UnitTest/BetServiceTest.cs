using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Domain.Interfaces.Repositories;
using Masiv.Casino.Domain.Services.Services;
using Masiv.Casino.Domain.Services.Utilities;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Masiv.Casino.Test.UnitTest
{
    public class BetServiceTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IBetRepository> mockBetRepository;
        private readonly IOptions<AppSettings> appSettings;
        private readonly BetService betService;
        private const string ROULETTE_KEY = "roulette_key";
        private const string BET_KEY = "bet_key";

        public BetServiceTest()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockBetRepository = mockRepository.Create<IBetRepository>();
            appSettings = Options.Create(new AppSettings { BetCacheKey = BET_KEY, RouletteCacheKey = ROULETTE_KEY });
            betService = new BetService(mockBetRepository.Object, appSettings);
        }

        [Theory]
        [InlineData("{\"Id\":\"b4ef4d9292c440a8a5b2bad30f89743d\",\"RouletteId\":\"75ba\",\"UserId\":\"edwin\",\"Number\":null,\"Color\":\"red\",\"Quantity\":6000,\"TotalBetWinner\":0}", "{\"Success\":true, \"Data\":null}", 1)]
        [InlineData("{\"Id\":\"b4ef4d9292c440a8a5b2bad30f89743d\",\"RouletteId\":\"75ba\",\"UserId\":\"edwin\",\"Number\":null,\"Color\":\"red\",\"Quantity\":6000,\"TotalBetWinner\":0}", "{\"Success\":false, \"Data\":null}", 2)]
        public async Task CreateBet_Test(string bet, string expectedResponse, int dbResponse)
        {
            Bet betInput = JsonSerializer.Deserialize<Bet>(bet);
            GenericResponse genericResponseExpected = JsonSerializer.Deserialize<GenericResponse>(expectedResponse);
            try
            {
                mockBetRepository.Setup(s => s.Save(betInput)).Returns(Task.FromResult(dbResponse));
                var response = await betService.Create(betInput);
                Assert.Equal(response.Success, genericResponseExpected.Success);
            }
            catch (BadRequest ex)
            {
                Assert.Equal(Constants.ROULETTE_NOT_OPEN, ex.ResultCode);
                Assert.NotNull(ex.Message);
            }

        }
    }
}