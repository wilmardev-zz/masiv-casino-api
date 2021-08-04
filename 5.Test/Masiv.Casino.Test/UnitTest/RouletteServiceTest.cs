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
    public class RouletteServiceTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IRouletteRepository> mockRouletteRepository;
        private readonly Mock<IBetRepository> mockBetRepository;
        private readonly IOptions<AppSettings> appSettings;
        private readonly RouletteService rouletteService;
        private const string ROULETTE_KEY = "roulette_key";
        private const string BET_KEY = "bet_key";

        public RouletteServiceTest()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockRouletteRepository = mockRepository.Create<IRouletteRepository>();
            mockBetRepository = mockRepository.Create<IBetRepository>();
            appSettings = Options.Create(new AppSettings { BetCacheKey = BET_KEY, RouletteCacheKey = ROULETTE_KEY });
            rouletteService = new RouletteService(mockRouletteRepository.Object, mockBetRepository.Object, appSettings);
        }

        [Fact]
        public async Task CreateRoulette_Test()
        {
            mockRouletteRepository.Setup(s => s.Save<Roulette>(new Roulette { })).Returns(Task.FromResult(new Roulette { }));
            var response = await rouletteService.Create();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetRoulette_Test()
        {
            List<Roulette> roulettesExpect = new List<Roulette>();
            mockRouletteRepository.Setup(s => s.Get()).Returns(Task.FromResult(roulettesExpect));
            var response = await rouletteService.Get();
            Assert.Equal(response.GetType().FullName, new List<Roulette> { }.GetType().FullName);
        }

        [Theory]
        [InlineData("4148645", "{\"Success\":true, \"Data\":null}", 1)]
        [InlineData("4148645", "{\"Success\":false, \"Data\":null}", 3)]
        public async Task OpenRoulette_Test(string rouletteId, string expectResponse, int dbResponse)
        {
            GenericResponse responseExpec = JsonSerializer.Deserialize<GenericResponse>(expectResponse);
            try
            {
                mockRouletteRepository.Setup(s => s.Open(rouletteId)).Returns(Task.FromResult(dbResponse));
                var response = await rouletteService.Open(rouletteId);
                Assert.Equal(response.Success, responseExpec.Success);
            }
            catch (BadRequest ex)
            {
                Assert.Equal(Constants.ROULETTE_IS_CLOSED, ex.ResultCode);
                Assert.NotNull(ex.Message);
            }

        }

        [Theory]
        [InlineData("4148645", "{\"Success\":true, \"Data\":null}", 1)]
        [InlineData("4148645", "{\"Success\":false, \"Data\":null}", 2)]
        public async Task CloseRoulette_Test(string rouletteId, string expectResponse, int expectDbResponse)
        {
            GenericResponse responseExpec = JsonSerializer.Deserialize<GenericResponse>(expectResponse);
            try
            {
                mockBetRepository.Setup(s => s.GetByRouletteId(rouletteId)).Returns(Task.FromResult(new List<Bet> { new Bet { RouletteId = rouletteId } }));
                mockRouletteRepository.Setup(s => s.Close(rouletteId)).Returns(Task.FromResult(expectDbResponse));
                var response = await rouletteService.Close(rouletteId);
                Assert.Equal(response.Success, responseExpec.Success);
                Assert.NotNull(response.Data);
            }
            catch (BadRequest ex)
            {
                Assert.Equal(Constants.ROULETTE_ALREADY_OPEN, ex.ResultCode);
                Assert.NotNull(ex.Message);
            }

        }
    }
}