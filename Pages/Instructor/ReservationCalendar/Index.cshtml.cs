using CengReservation.Data;
using CengReservation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CengReservation.Pages.Instructor.ReservationCalendar
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Classroom> ClassroomList { get; set; } = new();

        [BindProperty]
        public InputModel ReservationInput { get; set; } = new();

        public class InputModel
        {
            [Required]
            public DateTime ReservationDate { get; set; }

            [Required]
            public TimeSpan StartTime { get; set; }

            [Required]
            public TimeSpan EndTime { get; set; }

            [Required]
            public int ClassroomId { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ClassroomList = await _context.Classrooms.OrderBy(c => c.Name).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                ClassroomList = await _context.Classrooms.ToListAsync();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var currentTerm = await _context.Terms.FirstOrDefaultAsync(t =>
                t.StartDate <= ReservationInput.ReservationDate &&
                t.EndDate >= ReservationInput.ReservationDate);

            if (currentTerm == null)
            {
                ModelState.AddModelError("", "No active term found.");
                ClassroomList = await _context.Classrooms.ToListAsync();
                return Page();
            }

            var newReservation = new Reservation
            {
                UserId = user.Id,
                ClassroomId = ReservationInput.ClassroomId,
                TermId = currentTerm.Id,
                DayOfWeek = ReservationInput.ReservationDate.DayOfWeek,
                StartTime = ReservationInput.StartTime,
                EndTime = ReservationInput.EndTime,
                IsApproved = false,
                IsConflict = false,
                IsHoliday = false,
                CreatedAt = ReservationInput.ReservationDate, // Takvimde doğru görünmesi için
                Status = "Pending"
            };

            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetReservationsAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var reservations = await _context.Reservations
                .Include(r => r.Classroom)
                .Include(r => r.Term)
                .Where(r => r.UserId == user.Id)
                .ToListAsync();

            var events = reservations.Select(r =>
            {
                var start = r.CreatedAt.Date.Add(r.StartTime);
                var end = r.CreatedAt.Date.Add(r.EndTime);

                string color = r.Status switch
                {
                    "Approved" => "green",
                    "Rejected" => "red",
                    _ => "orange"
                };

                return new
                {
                    id = r.Id,
                    title = $"{r.Classroom.Name} ({r.StartTime:hh\\:mm}-{r.EndTime:hh\\:mm})",
                    start = start.ToString("s"),
                    end = end.ToString("s"),
                    color = color,
                    extendedProps = new { classroomId = r.ClassroomId }
                };
            });

            return new JsonResult(events);
        }
    }
}
