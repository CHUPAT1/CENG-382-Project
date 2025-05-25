using System.ComponentModel.DataAnnotations;

namespace CengReservation.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!; 
        public ApplicationUser User { get; set; } = null!;

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;

        public int TermId { get; set; }
        public Term Term { get; set; } = null!;

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsApproved { get; set; }
        public bool IsConflict { get; set; }
        public bool IsHoliday { get; set; }

        public string Status{ get; set; } = "Pending"; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
