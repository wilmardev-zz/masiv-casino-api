using Masiv.Casino.Application.Contracts.DTO;
using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Masiv.Casino.WebApi.Controllers
{
    [ApiController]
    public class BetController : Controller
    {
        private readonly IBetApplication betApplication;
        private readonly AppSettings appSettings;

        public BetController(IBetApplication betApplication, IOptions<AppSettings> appSettings)
        {
            this.betApplication = betApplication;
            this.appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("casino/bet/create")]
        public async Task<object> Create([FromHeader(Name = "userId")][Required] string userId, [FromBody] BetDTO betDTO)
        {
            if (!ValidateRequest(betDTO))
                return BadRequest();
            return await betApplication.Create(betDTO, userId);
        }

        private bool ValidateRequest(BetDTO betDTO)
        {
            if (!string.IsNullOrEmpty(betDTO.Color) &&
                !BetColor.Black.ToString().ToUpper().Equals(betDTO.Color.ToUpper()) &&
                !BetColor.Red.ToString().ToUpper().Equals(betDTO.Color.ToUpper()))
                return false;
            if (!(betDTO.Number == null ||
                (betDTO.Number >= appSettings.MinBetValidNumber &&
                betDTO.Number <= appSettings.MaxBetValidNumber)))
                return false;
            if (betDTO.Quantity <= 0 || betDTO.Quantity > appSettings.MaxBetQuantityPerRoulette)
                return false;
            return true;
        }
    }
}