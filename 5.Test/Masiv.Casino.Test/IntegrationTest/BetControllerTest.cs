using Masiv.Casino.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Masiv.Casino.Test.IntegrationTest
{
    public class BetControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public BetControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData("casino/bet/create", "{\"RouletteId\":\"75bac53fdd84414caf5146709ea24236\",\"Number\":null,\"Color\":\"red\",\"Quantity\":6000}","wilmar", 200)]
        [InlineData("casino/bet/create", "{\"RouletteId\":\"75bac53fdd84414caf5146709ea24236\",\"Number\":null,\"Color\":\"red\",\"Quantity\":6000}","", 400)]
        [InlineData("casino/bet/create", "", "", 400)]
        [InlineData("casino/bet/create", "{\"RouletteId\":null,\"Number\":null,\"Color\":\"red\",\"Quantity\":6000}", "", 400)]
        [InlineData("casino/bet/create", "{\"RouletteId\":\"75bac53fdd84414caf5146709ea24236\",\"Number\":null,\"Color\":null,\"Quantity\":6000}", "", 400)]
        [InlineData("casino/bet/create", "{\"RouletteId\":\"75bac53fdd84414caf5146709ea24236\",\"Number\":13,\"Color\":null,\"Quantity\":0}", "", 400)]
        [InlineData("casino/bet/create", "{\"RouletteId\":\"75bac53fdd84414caf5146709ea24236\",\"Number\":13,\"Color\":null,\"Quantity\":10001}", "", 400)]
        public async Task CreateBetMethod_Test(string url, string jsonInput, string headers, int statusExpected)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("userId", headers);
            StringContent inputData = new StringContent(jsonInput, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, inputData);
            Assert.Equal(statusExpected, (int)response.StatusCode);
        }
    }
}