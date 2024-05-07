using TourPlaner.Models;

namespace TourPlaner.DAL
{
    public interface ITourRepository
    {
        void AddTour(TourModel tour);
    }
}