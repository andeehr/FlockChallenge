using Flock.Common.Domain;

namespace Flock.Common.Services.Interfaces
{
    public interface IUserService
    {
        User Login(string username, string password);
        User GetById(int id);
    }
}
