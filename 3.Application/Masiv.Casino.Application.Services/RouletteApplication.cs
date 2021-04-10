using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Interfaces.Services;
using Masiv.Casino.Domain.Services.Utilities;
using System.Threading.Tasks;

namespace Masiv.Casino.Application.Services
{
    public class RouletteApplication : IRouletteApplication
    {
        private readonly IRouletteService rouletteService;

        public RouletteApplication(IRouletteService rouletteService)
        {
            this.rouletteService = rouletteService;
        }

        public async Task<GenericResponse> Close(string rouletteId)
        {
            return await rouletteService.Close(rouletteId);
        }

        public async Task<GenericResponse> Create()
        {
            var rouletteId = await rouletteService.Create();
            return Helper.ManageResponse(rouletteId);
        }

        public async Task<GenericResponse> Get()
        {
            var rouletteList = await rouletteService.Get();
            return Helper.ManageResponse(rouletteList);
        }

        public async Task<GenericResponse> Open(string rouletteId)
        {
            return await rouletteService.Open(rouletteId);
        }
    }
}