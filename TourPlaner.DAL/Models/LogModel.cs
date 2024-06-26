namespace TourPlaner.Models
{
    public class LogModel
    
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string TotalTime { get; set; } = "";
        public string TotalDistance { get; set; } = "";
    }
}