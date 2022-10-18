using Microsoft.EntityFrameworkCore;
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

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await context.Platforms.ToListAsync();
        }

        public async Task<Platform?> GetPlatformById(int id)
        {
            return await context.Platforms.FirstOrDefaultAsync(platform => platform.Id == id);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null) 
            {
                throw new ArgumentNullException(nameof(platform));
            }

            context.Platforms.Add(platform);
        }
    }
}