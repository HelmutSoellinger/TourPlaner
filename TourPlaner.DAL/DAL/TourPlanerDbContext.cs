using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TourPlaner.Models;

namespace TourPlaner.DAL
{
    public class TourPlanerDbContext : DbContext, ITourPlanerDbContext
    {
        public DbSet<TourModel> Tours { get; set; }
        public DbSet<LogModel> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            optionsBuilder.UseNpgsql("Username=postgres;Password=changeme;Host=localhost;Port=5432;Database=TourPlanerDb;");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // notwendig da DateTime.Now sonst nicht geht
            // optionsBuilder.UseSqlServer("Initial Catalog=Sample;User=sa;Password=pass@word1;Data Source=localhost;MultipleActiveResultSets=True",
            // sopt => sopt.UseNetTopologySuite());
        }
    }
}
