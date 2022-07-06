using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Platform> Platforms => Set<Platform>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseInMemoryDatabase("PlatformService");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>().HasData(
                new Platform { Id = 1, Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Id = 2, Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Id = 3, Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );
        }
    }
}