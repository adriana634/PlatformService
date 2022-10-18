using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        Task SaveChangesAsync();
        Task<IEnumerable<Platform>> GetAllPlatforms();
        Task<Platform?> GetPlatformById(int id);
        void CreatePlatform(Platform platform);
    }
}