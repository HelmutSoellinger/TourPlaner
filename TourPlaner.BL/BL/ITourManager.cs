using System.Collections.ObjectModel;
using TourPlaner.DAL;
using TourPlaner.Models;

namespace TourPlaner.BL
{
    public interface ITourManager
    {
        public ObservableCollection<TourModel> Tours { get; set; }
        public void AddTour(TourModel tour);
        public void AddLog(int tourId, LogModel log);
        public void RemoveTour(TourModel tourId);
        public void RemoveLog(LogModel logId);
        public void Update();
        public ObservableCollection<TourModel> RetrieveTours();
    }
}