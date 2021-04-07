using Masiv.Casino.Application.Interfaces;
using Masiv.Casino.Application.Services;
using Masiv.Casino.Domain.Interfaces.Repositories;
using Masiv.Casino.Domain.Interfaces.Services;
using Masiv.Casino.Domain.Services.Services;
using Masiv.Casino.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Masiv.Casino.Infra.IoC
{
    public class DependencyInjector
    {
        public DependencyInjector()
        {
        }

        public IServiceCollection GetServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<ICacheRepository, CacheRepository>();
            services.AddSingleton<IBetService, BetService>();
            services.AddSingleton<IRouletteService, RouletteService>();
            services.AddSingleton<IBetApplication, BetApplication>();
            services.AddSingleton<IRouletteApplication, RouletteApplication>();

            return services;
        }
    }
}