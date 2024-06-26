using TourPlaner.Models;
using TourPlaner.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TourPlaner.BL
{
    public class TourManager : ITourManager
    {
        private readonly ITourRepository tourRepository;
        public TourManager(ITourRepository tourRepository)
        {
            this.tourRepository = tourRepository;
        }
        public void AddTour(TourModel tour)
        {
            tourRepository.AddTour(tour);
            // Add a new tour
        }
        public void AddLog(int tourId, LogModel log)
        {
            tourRepository.AddLog(tourId, log);
        }
        public void RemoveTour(TourModel tour)
        {
            tourRepository.RemoveTour(tour.Id);
        }
        public void RemoveLog(LogModel log)
        {
            tourRepository.RemoveLog(log.Id);
        }
        public ObservableCollection<TourModel> retrieveTours()
        {
            return tourRepository.retrieveTours();
        }
    }
}
