using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Masiv.Casino.WebApi.Controllers
{
    [ApiController]
    public class RouletteController : Controller
    {
        [HttpPost]
        [Route("roulette/open")]
        public async Task<IActionResult> Open()
        {
            return Ok();
        }
    }
}