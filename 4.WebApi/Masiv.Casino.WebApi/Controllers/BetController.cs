using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Masiv.Casino.WebApi.Controllers
{
    [ApiController]
    public class BetController : Controller
    {
        private readonly IBetApplication betApplication;

        public BetController(IBetApplication betApplication)
        {
            this.betApplication = betApplication;
        }

        [HttpPost]
        [Route("bet")]
        public async Task<GenericResponse> Create([FromBody] Bet bet)
        {
            return await betApplication.Create(bet);
        }
    }
}