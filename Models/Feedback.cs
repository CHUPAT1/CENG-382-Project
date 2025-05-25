using System.ComponentModel.DataAnnotations;

namespace CengReservation.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } 

        [Range(1, 5)]
        public int Rating { get; set; } 
        public string Comment { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}