using AutoMapper;
using Flock.API.Models;
using Flock.Common.Domain;

namespace Flock.API.Config.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>();
        }
    }
}
