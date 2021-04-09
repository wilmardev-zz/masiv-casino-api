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
        [Route("roulette/create")]
        public async Task<GenericResponse> Create()
        {
            return await rouletteApplication.Create();
        }

        [HttpGet]
        [Route("roulette/all")]
        public async Task<GenericResponse> Get()
        {
            return await rouletteApplication.Get();
        }

        [HttpPost]
        [Route("roulette/open")]
        public async Task<GenericResponse> Open(Roulette roulette)
        {
            return await rouletteApplication.Open(roulette);
        }

        [HttpPost]
        [Route("roulette/close")]
        public async Task<GenericResponse> Close(Roulette roulette)
        {
            return await rouletteApplication.Close(roulette);
        }
    }
}