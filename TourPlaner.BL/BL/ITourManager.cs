using System.Collections.ObjectModel;
using TourPlaner.DAL;
using TourPlaner.Models;

namespace TourPlaner.BL
{
    public interface ITourManager
    {
        void AddTour(TourModel tour);
        void AddLog(int tourId, LogModel log);
        public void RemoveTour(TourModel tourId);
        public void RemoveLog(LogModel logId);
        ObservableCollection<TourModel> retrieveTours();
    }
}