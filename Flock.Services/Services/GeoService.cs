using Flock.Common.Domain;
using Flock.Common.Exceptions;
using Flock.Common.Helpers;
using Flock.Common.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Flock.Services.Services
{
    public class GeoService : IGeoService
    {
        private readonly HttpClient client;
        private readonly ILogger<GeoService> logger;
        public GeoService(IHttpClientFactory factory, ILogger<GeoService> logger)
        {
            this.client = factory.CreateClient("geoApi");
            this.logger = logger;
        }

        private GeoData GetGeoData(string provincia)
        {
            var query = HttpUtility.ParseQueryString("");
            query["nombre"] = provincia;

            var endpoint = UrlHelper.BuildUrl(client.BaseAddress.AbsoluteUri, Constants.SECTION_PROVINCIAS, query);

            logger.LogInformation($"Obteniendo datos de url {endpoint}");

            var response = client.GetAsync(endpoint);
            response.Wait();
            var result = response.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsStringAsync();
                readTask.Wait();
                return JsonConvert.DeserializeObject<GeoData>(readTask.Result);
            }
            logger.LogInformation($"La api respondió con status code {result.StatusCode}");
            return null;
        }

        public Centroide GetCoordenadas(string provincia)
        {
            try
            {
                var data = GetGeoData(provincia);

                if (data != null && data.Provincias.Any())
                {
                    if (data.Cantidad > 1)
                    {
                        throw new ValidationException($"Se encontró más de un resultado con el nombre '{provincia}' ({GetNombres(data.Provincias)}).");
                    }
                    return data.Provincias.First().Centroide;
                }
                throw new ValidationException($"No existen provincias con el nombre {provincia}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Ocurrió un error al obtener las coordenadas: {ex.Message}");
                throw;
            }
        }

        private static string GetNombres(Provincia[] provincias)
        {
            return string.Join(", ", provincias.Select(p => p.Nombre));
        }
    }
}
