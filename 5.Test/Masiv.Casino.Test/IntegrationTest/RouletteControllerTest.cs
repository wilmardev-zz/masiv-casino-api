using Masiv.Casino.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace Masiv.Casino.Test.IntegrationTest
{
    public class RouletteControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public RouletteControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData("casino/roulette/all", 200)]
        public async Task GetAllRouletteMethod_Test(string url, int expectedCode)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(url);
            Assert.Equal(expectedCode, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("casino/roulette/create",  200)]
        public async Task CreateRouletteMethod_Test(string url, int expectedCode)
        {
            var client = factory.CreateClient();
            var response = await client.PostAsync(url, null);
            Assert.Equal(expectedCode, (int)response.StatusCode);
        }

        [Theory]
        //[InlineData("casino/roulette/{rouletteId}/open", "B1AABCDB-F39D-433F-B8BB-96CAB73253E4", 200)]
        [InlineData("casino/roulette/{rouletteId}/open", "DCS", 400)]
        //[InlineData("casino/roulette/{rouletteId}/close", "B1AABCDB-F39D-433F-B8BB-96CAB73253E4", 200)]
        [InlineData("casino/roulette/{rouletteId}/close", "SDSF", 400)]
        public async Task CloseOpenRouletteMethod_Test(string url, string rouletteId, int expectedCode)
        {
            var client = factory.CreateClient();
            var response = await client.PutAsync(url.Replace("{rouletteId}", rouletteId), null);
            Assert.Equal(expectedCode, (int)response.StatusCode);
        }
    }
}