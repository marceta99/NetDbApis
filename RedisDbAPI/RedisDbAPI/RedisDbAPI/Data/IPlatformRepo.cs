using RedisDbAPI.Models;

namespace RedisDbAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatform(Platform plat);

        Platform? GetPlatformById(string id);

        IEnumerable<Platform> GetPlatforms();

    }
}
