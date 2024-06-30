using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TourPlaner.Models;

namespace TourPlaner.DAL
{
    public interface ITourRepository
    {
        public void AddTour(TourModel tour);
        public void AddLog(int tourId, LogModel log);
        public void RemoveTour(int tourId);
        public void RemoveLog(int logId);
        public void Update();
        public ObservableCollection<TourModel> RetrieveTours();
    }
}