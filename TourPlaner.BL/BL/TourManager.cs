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
        public ObservableCollection<TourModel> Tours { get; set; } = [];
        public TourManager(ITourRepository tourRepository)
        {
            this.tourRepository = tourRepository;
        }
        public void AddTour(TourModel tour)
        {
            tourRepository.AddTour(tour);
            Tours.Add(tour);
        }
        public void AddLog(int tourId, LogModel log)
        {
            tourRepository.AddLog(tourId, log);
        }
        public void RemoveTour(TourModel tour)
        {
            tourRepository.RemoveTour(tour.Id);
            Tours.Remove(tour);
        }
        public void RemoveLog(LogModel log)
        {
            tourRepository.RemoveLog(log.Id);
        }
        public void Update()
        {
            tourRepository.Update();
        }
        public ObservableCollection<TourModel> RetrieveTours()
        {
            return tourRepository.RetrieveTours();
        }
    }
}
