using AutoMapper;
using Flock.API.Models.GeoData;
using Flock.Common.Services.Interfaces;
using FlockAPI.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Flock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoController : ApiController
    {
        private readonly IGeoService service;
        public GeoController(IWebHostEnvironment environment, ILogger<GeoController> logger, IMapper mapper, IGeoService service) : base(environment, logger, mapper)
        {
            this.service = service;
        }

        [HttpGet("coordenadas/{provincia}")]
        public ActionResult GetCoordenadas(string provincia)
        {
            var result = service.GetCoordenadas(provincia);
            if (result != null)
            {
                return Ok(Mapper.Map<CentroideModel>(result));
            }
            return NotFound();
        }
    }
}
