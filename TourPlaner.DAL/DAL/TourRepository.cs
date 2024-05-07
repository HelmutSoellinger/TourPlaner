using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.DAL
{
    public class TourRepository : ITourRepository
    {
        private readonly ITourPlanerDbContext dbContext;

        public TourRepository(ITourPlanerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddTour(TourModel tour)
        {
            dbContext.Tours.Add(tour);
            dbContext.SaveChanges();
        }
    }
}
