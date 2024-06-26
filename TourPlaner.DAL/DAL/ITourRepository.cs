using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TourPlaner.Models;

namespace TourPlaner.DAL
{
    public interface ITourRepository
    {
        void AddTour(TourModel tour);
        void AddLog(int tourId, LogModel log);
        public void RemoveTour(int tourId);
        public void RemoveLog(int logId);
        ObservableCollection<TourModel> retrieveTours();
    }
}