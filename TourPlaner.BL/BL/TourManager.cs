using TourPlaner.Models;
using TourPlaner.DAL;

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
    }
}
