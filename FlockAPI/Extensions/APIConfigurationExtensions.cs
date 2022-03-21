using Flock.API.Models;
using Flock.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Flock.API.Extensions
{
    public static class APIConfigurationExtensions
    {
        public static IApplicationBuilder UseAPIStatusCodePages(this IApplicationBuilder app)
        {
            return app.UseStatusCodePages(async context => {
                var statusCode = context.HttpContext.Response.StatusCode;

                var vm = ErrorModel.FromStatusCode(statusCode);
                var content = vm.ToString();
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(content);
            });
        }

        public static IApplicationBuilder UseAPIExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(errorApp => {
                errorApp.Run(async context => {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    var vm = CreateErrorModel(exceptionHandlerPathFeature);

                    var content = vm.ToString();
                    context.Response.StatusCode = vm.StatusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(content);
                });
            });
        }

        private static ErrorModel CreateErrorModel(IExceptionHandlerPathFeature exceptionHandlerPathFeature)
        {
            if (exceptionHandlerPathFeature?.Error is ValidationException)
            {
                return new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = exceptionHandlerPathFeature.Error.Message,
                };
            }

            return new ErrorModel
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Ocurrió un error en la aplicación",
            };
        }

        public static IServiceCollection ConfigureApiBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = context => {
                    var vm = ErrorModel.FromModelState(context.ModelState);
                    return new ContentResult()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ContentType = "application/json",
                        Content = vm.ToString(),
                    };
                };
            });

            return services;
        }
    }
}
