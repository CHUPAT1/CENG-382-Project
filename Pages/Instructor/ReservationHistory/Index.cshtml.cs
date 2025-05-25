using CengReservation.Data;
using CengReservation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CengReservation.Pages.Instructor.ReservationHistory
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

        public List<Reservation> Reservations { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            Reservations = await _context.Reservations
                .Include(r => r.Classroom)
                .Include(r => r.Term)
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Page();
        }

        public DateTime GetClosestDate(DateTime start, DayOfWeek targetDay)
        {
            int diff = ((int)targetDay - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(diff);
        }
    }
}
