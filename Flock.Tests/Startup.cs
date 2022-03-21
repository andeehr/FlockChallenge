using Flock.Common.Services.Interfaces;
using Flock.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Flock.Tests
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IGeoService, GeoService>();
            services.AddHttpClient("geoApi", options => {
                options.BaseAddress = new Uri("https://apis.datos.gob.ar/georef/api/");
            });
        }
    }
}
