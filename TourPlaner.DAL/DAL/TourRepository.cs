using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.Models;

namespace TourPlaner.DAL
{
    public class TourRepository : ITourRepository
    {
        private readonly TourPlanerDbContext dbContext;

        public TourRepository(TourPlanerDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        public void AddTour(TourModel tour)
        {
            dbContext.Tours.Add(tour);
            dbContext.SaveChanges();
        }
        public void AddLog(int tourId, LogModel log)
        {
            var tour = dbContext.Tours.Find(tourId);
            if (tour != null && tour.Logs == null)
            {
                tour.Logs = new ObservableCollection<LogModel>();
            }
            tour.Logs?.Add(log);
            dbContext.SaveChanges();
        }
        public void RemoveTour(int tourId)
        {
            // Find the tour by ID
            var tour = dbContext.Tours.Include(t => t.Logs).FirstOrDefault(t => t.Id == tourId);

            if (tour != null)
            {
                // Remove all logs associated with the tour
                foreach (var log in tour.Logs.ToList())
                {
                    dbContext.Logs.Remove(log);
                }

                // After removing all logs, remove the tour itself
                dbContext.Tours.Remove(tour);
                dbContext.SaveChanges();
            }
        }
        public void RemoveLog(int logId)
        {
            var logToRemove = dbContext.Logs.FirstOrDefault(l => l.Id == logId);
            if (logToRemove != null)
            {
                dbContext.Logs.Remove(logToRemove);
                dbContext.SaveChanges();
            }
        }
        public ObservableCollection<TourModel> retrieveTours()
        {
            var tours = dbContext.Tours.Include(t => t.Logs).ToList();
            return new ObservableCollection<TourModel>(tours);
        }
    }
}
