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
        public async Task<object> Create([FromHeader(Name = "userId")][Required] string userId, [FromBody] BetDto betDto)
        {
            if (!ValidateRequest(betDto))
                return BadRequest();
            return await betApplication.Create(betDto, userId);
        }

        private bool ValidateRequest(BetDto betDto)
        {
            if (!string.IsNullOrEmpty(betDto.Color) &&
                !BetColor.Black.ToString().ToUpper().Equals(betDto.Color.ToUpper()) &&
                !BetColor.Red.ToString().ToUpper().Equals(betDto.Color.ToUpper()))
                return false;
            if (!(betDto.Number == null ||
                (betDto.Number >= appSettings.MinBetValidNumber &&
                betDto.Number <= appSettings.MaxBetValidNumber)))
                return false;
            if (betDto.Quantity <= 0 || betDto.Quantity > appSettings.MaxBetQuantityPerRoulette)
                return false;
            return true;
        }
    }
}