using Flock.Common.Domain;

namespace Flock.Common.Services.Interfaces
{
    public interface IGeoService
    {
        Centroide GetCoordenadas(string provincia);
    }
}
