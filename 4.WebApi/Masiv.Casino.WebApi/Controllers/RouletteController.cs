using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Masiv.Casino.WebApi.Controllers
{
    [ApiController]
    public class RouletteController : Controller
    {
        private readonly IRouletteApplication rouletteApplication;

        public RouletteController(IRouletteApplication rouletteApplication)
        {
            this.rouletteApplication = rouletteApplication;
        }

        [HttpPost]
        [Route("casino/roulette/create")]
        public async Task<GenericResponse> Create()
        {
            return await rouletteApplication.Create();
        }

        [HttpGet]
        [Route("casino/roulette/all")]
        public async Task<GenericResponse> Get()
        {
            return await rouletteApplication.Get();
        }

        [HttpPut]
        [Route("casino/roulette/{rouletteId}/open")]
        public async Task<GenericResponse> Open([FromRoute] string rouletteId)
        {
            return await rouletteApplication.Open(rouletteId);
        }

        [HttpPut]
        [Route("casino/roulette/{rouletteId}/close")]
        public async Task<GenericResponse> Close([FromRoute] string rouletteId)
        {
            return await rouletteApplication.Close(rouletteId);
        }
    }
}