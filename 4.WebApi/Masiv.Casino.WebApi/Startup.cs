using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Infra.IoC;
using Masiv.Casino.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;

namespace Masiv.Casino.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>((options) =>
            {
                return Configuration.GetSection("RedisConfig").Get<RedisConfiguration>();
            });
            services.Add(new DependencyInjector().GetServiceCollection());
            services.AddControllers();
            AddSwagger(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseRouting();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Casino API v1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/api/values", async context =>
                {
                    await context.Response.WriteAsync("Api Casino is running!!");
                });
                endpoints.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";
                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Casino API {groupName}",
                    Version = groupName,
                    Description = "Casino API",
                    Contact = new OpenApiContact
                    {
                        Name = "Wilmar Duque",
                        Email = "wilmarduque71@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/wilmarduque71/"),
                    }
                });
            });
        }
    }
}