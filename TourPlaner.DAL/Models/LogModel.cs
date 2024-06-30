namespace TourPlaner.Models
{
    public class LogModel
    
    {
        public int Id { get; set; }
        public string Date { get; set; } = DateTime.Now.ToString();
        public string TotalTime { get; set; } = "";
        public string TotalDistance { get; set; } = "";
        public string Comment { get; set; } = "";
        public string Difficulty { get; set; } = "";
        public string Rating { get; set; } = "";
    }
}