using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Platform> Platforms { get; set; }

        private readonly IConfiguration configuration;
        private readonly ILogger<AppDbContext> logger;

        public AppDbContext(IConfiguration configuration, ILogger<AppDbContext> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            logger.LogInformation("Using SQL Server database");

            var connectionString = configuration.GetConnectionString("Platforms");
            optionsBuilder.UseSqlServer(connectionString);
        }

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