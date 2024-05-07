using Microsoft.EntityFrameworkCore;
using TourPlaner.Models;

namespace TourPlaner.DAL
{
    public interface ITourPlanerDbContext
    {
        DbSet<LogModel> Logs { get; set; }
        DbSet<TourModel> Tours { get; set; }

        int SaveChanges();
    }
}