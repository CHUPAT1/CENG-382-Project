using System.ComponentModel.DataAnnotations;

namespace CengReservation.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}