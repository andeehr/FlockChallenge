using Flock.Common.Exceptions;
using Flock.Common.Services.Interfaces;
using System;
using Xunit;

namespace Flock.Tests
{
    public class GeoServiceTest
    {
        private readonly IGeoService geoService;

        public GeoServiceTest(IGeoService service) => geoService = service;

        [Fact]
        public void ProvinciaInexistenteTest()
        {
            Action action = () => geoService.GetCoordenadas("Montevideo");
            Assert.Throws<ValidationException>(action);
        }
    }
}
