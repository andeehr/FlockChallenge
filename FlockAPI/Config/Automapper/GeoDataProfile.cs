using AutoMapper;
using Flock.API.Models.GeoData;
using Flock.Common.Domain;

namespace Flock.API.Config.Automapper
{
    public class GeoDataProfile : Profile
    {
        public GeoDataProfile()
        {
            CreateMap<Centroide, CentroideModel>()
                .ForMember(dest => dest.Latitud, opt => opt.MapFrom(src => src.Lat))
                .ForMember(dest => dest.Longitud, opt => opt.MapFrom(src => src.Lon))
                ;

            CreateMap<Provincia, ProvinciaModel>();
            CreateMap<GeoData, GeoDataModel>();
        }
    }
}
