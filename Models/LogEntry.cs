using System.ComponentModel.DataAnnotations;

namespace CengReservation.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}