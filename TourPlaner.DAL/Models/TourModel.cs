using System.Collections.ObjectModel;

namespace TourPlaner.Models
{
    public class TourModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string StartLocation { get; set; } = "";
        public string EndLocation { get; set; } = "";
        public string RouteInformation { get; set; } = "";
        public string Distance { get; set; } = "";
        public string FileName { get; set; } = "";
        public ObservableCollection<LogModel> Logs { get; set; } = new ObservableCollection<LogModel>();
    }
}