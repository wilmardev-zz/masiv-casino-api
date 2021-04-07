using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Masiv.Casino.WebApi.Controllers
{
    [ApiController]
    public class BetController : Controller
    {
        [HttpPost]
        [Route("bet/create")]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }
    }
}