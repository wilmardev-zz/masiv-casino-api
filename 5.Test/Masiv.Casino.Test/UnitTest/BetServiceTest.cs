using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Domain.Interfaces.Repositories;
using Masiv.Casino.Domain.Services.Services;
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
        private readonly Mock<ICacheRepository> mockCacheRepository;
        private readonly IOptions<AppSettings> appSettings;
        private readonly BetService betService;
        private const string ROULETTE_KEY = "roulette_key";
        private const string BET_KEY = "bet_key";

        public BetServiceTest()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockCacheRepository = mockRepository.Create<ICacheRepository>();
            appSettings = Options.Create(new AppSettings { BetCacheKey = BET_KEY, RouletteCacheKey = ROULETTE_KEY });
            betService = new BetService(mockCacheRepository.Object, appSettings);
        }

        [Theory]
        [InlineData("{\"Id\":\"b4ef4d9292c440a8a5b2bad30f89743d\",\"RouletteId\":\"75ba\",\"UserId\":\"edwin\",\"Number\":null,\"Color\":\"red\",\"Quantity\":6000,\"TotalBetWinner\":0}", "{\"Success\":true, \"Data\":null}", "Open")]
        [InlineData("{\"Id\":\"b4ef4d9292c440a8a5b2bad30f89743d\",\"RouletteId\":\"75ba\",\"UserId\":\"edwin\",\"Number\":null,\"Color\":\"red\",\"Quantity\":6000,\"TotalBetWinner\":0}", "{\"Success\":false, \"Data\":null}", "Close")]
        public async Task CreateBet_Test(string bet, string expectedResponse, string rouletteStatus)
        {
            Bet betInput = JsonSerializer.Deserialize<Bet>(bet);
            GenericResponse genericResponseExpected = JsonSerializer.Deserialize<GenericResponse>(expectedResponse);
            mockCacheRepository.Setup(s => s.Get<Bet>(BET_KEY)).Returns(Task.FromResult(new List<Bet> { new Bet() }));
            mockCacheRepository.Setup(s => s.Get<Roulette>(ROULETTE_KEY)).Returns(Task.FromResult(new List<Roulette> { new Roulette { Id = betInput.RouletteId, State = rouletteStatus } }));
            mockCacheRepository.Setup(s => s.Save(new List<Bet> { betInput }, BET_KEY)).Returns(Task.FromResult(genericResponseExpected));
            var response = await betService.Create(betInput);
            Assert.Equal(response.Success, genericResponseExpected.Success);
        }

        [Fact]
        public async Task GetBet_Test()
        {
            List<Bet> betsMock = new List<Bet>();
            mockCacheRepository.Setup(s => s.Get<Bet>(BET_KEY)).Returns(Task.FromResult(betsMock));
            var response = await betService.Get();
            Assert.Equal(response.GetType().FullName, new List<Bet> { }.GetType().FullName);
        }
    }
}