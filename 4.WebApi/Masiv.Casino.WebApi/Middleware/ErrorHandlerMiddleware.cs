using Masiv.Casino.Domain.Entities;
using Masiv.Casino.Domain.Entities.Config;
using Masiv.Casino.Domain.Services.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Masiv.Casino.WebApi.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError($"-- Error: {ex.Message}  --- Stack Trace : {ex.StackTrace}");
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = 500;
                ErrorResponse errorResponse = new ErrorResponse
                {
                    ResultCode = "INTERNAL_SERVER_ERROR",
                    ResultMsg = "Something went wrong. Please try again."
                };
                GenericResponse genericResponse = Helper.ManageResponse(errorResponse, false);
                var result = JsonSerializer.Serialize(genericResponse);
                await response.WriteAsync(result);
            }
        }
    }
}