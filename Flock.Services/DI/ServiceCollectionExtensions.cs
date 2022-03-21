using Flock.Common.Services.Interfaces;
using Flock.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Flock.Services.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGeoService, GeoService>();

            return services;
        }
    }
}
