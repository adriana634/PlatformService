using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext context;

        public PlatformRepo(AppDbContext context)
        {
            this.context = context;
        }

        public bool SaveChanges()
        {
            return (this.context.SaveChanges() >= 0);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return this.context.Platforms.ToList();
        }

        public Platform? GetPlatformById(int id)
        {
            return this.context.Platforms.FirstOrDefault(platform => platform.Id == id);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null) throw new ArgumentNullException(nameof(platform));

            this.context.Platforms.Add(platform);
        }
    }
}