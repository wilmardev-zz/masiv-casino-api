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
    public class RouletteServiceTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<ICacheRepository> mockCacheRepository;
        private readonly IOptions<AppSettings> appSettings;
        private readonly RouletteService rouletteService;
        private const string ROULETTE_KEY = "roulette_key";
        private const string BET_KEY = "bet_key";

        public RouletteServiceTest()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockCacheRepository = mockRepository.Create<ICacheRepository>();
            appSettings = Options.Create(new AppSettings { BetCacheKey = BET_KEY, RouletteCacheKey = ROULETTE_KEY });
            rouletteService = new RouletteService(mockCacheRepository.Object, appSettings);
        }

        [Fact]
        public async Task CreateRoulette_Test()
        {
            mockCacheRepository.Setup(s => s.Get<Roulette>(ROULETTE_KEY)).Returns(Task.FromResult(new List<Roulette> { }));
            mockCacheRepository.Setup(s => s.Save(new List<Roulette> { }, ROULETTE_KEY)).Returns(Task.FromResult(new Roulette { }));
            var response = await rouletteService.Create();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetRoulette_Test()
        {
            List<Roulette> roulettesExpect = new List<Roulette>();
            mockCacheRepository.Setup(s => s.Get<Roulette>(ROULETTE_KEY)).Returns(Task.FromResult(roulettesExpect));
            var response = await rouletteService.Get();
            Assert.Equal(response.GetType().FullName, new List<Roulette> { }.GetType().FullName);
        }

        [Theory]
        [InlineData("4148645", "{\"Success\":true, \"Data\":null}", "Pending")]
        [InlineData("4148645", "{\"Success\":false, \"Data\":null}", "Close")]
        public async Task OpenRoulette_Test(string rouletteId, string expectResponse, string rouletteStatus)
        {
            GenericResponse responseExpec = JsonSerializer.Deserialize<GenericResponse>(expectResponse);
            List<Roulette> roulettesExpect = new List<Roulette> { new Roulette { Id = rouletteId, State = rouletteStatus } };
            mockCacheRepository.Setup(s => s.Get<Roulette>(ROULETTE_KEY)).Returns(Task.FromResult(roulettesExpect));
            mockCacheRepository.Setup(s => s.Save(roulettesExpect, ROULETTE_KEY)).Returns(Task.FromResult(roulettesExpect));
            var response = await rouletteService.Open(rouletteId);
            Assert.Equal(response.Success, responseExpec.Success);
        }

        [Theory]
        [InlineData("4148645", "{\"Success\":true, \"Data\":null}", "Open")]
        [InlineData("4148645", "{\"Success\":false, \"Data\":null}", "Close")]
        public async Task CloseRoulette_Test(string rouletteId, string expectResponse, string rouletteStatus)
        {
            GenericResponse responseExpec = JsonSerializer.Deserialize<GenericResponse>(expectResponse);
            List<Roulette> roulettesExpect = new List<Roulette> { new Roulette { Id = rouletteId, State = rouletteStatus } };
            mockCacheRepository.Setup(s => s.Get<Bet>(BET_KEY)).Returns(Task.FromResult(new List<Bet> { new Bet { RouletteId = rouletteId } }));
            mockCacheRepository.Setup(s => s.Get<Roulette>(ROULETTE_KEY)).Returns(Task.FromResult(roulettesExpect));
            mockCacheRepository.Setup(s => s.Save(roulettesExpect, ROULETTE_KEY)).Returns(Task.FromResult(roulettesExpect));
            var response = await rouletteService.Close(rouletteId);
            Assert.Equal(response.Success, responseExpec.Success);
            Assert.NotNull(response.Data);
        }
    }
}