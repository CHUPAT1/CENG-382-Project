using CengReservation.Data;
using CengReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CengReservation.Pages.Admin.ReservationCalendar
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Reservation> Reservations { get; set; } = new();

        [BindProperty]
        public int ReservationId { get; set; }

        [BindProperty]
        public string ActionType { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            Reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Classroom)
                .Include(r => r.Term)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync()
        {
            var reservation = await _context.Reservations.FindAsync(ReservationId);
            if (reservation == null)
                return NotFound();

            switch (ActionType)
            {
                case "Approve":
                    reservation.IsApproved = true;
                    reservation.Status = "Approved";
                    break;
                case "Reject":
                    reservation.IsApproved = false;
                    reservation.Status = "Rejected";
                    break;
                case "Delete":
                    _context.Reservations.Remove(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToPage();
            }

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public DateTime GetNextDateForDayOfWeek(DateTime termStart, DayOfWeek targetDay)
        {
            int daysUntil = ((int)targetDay - (int)termStart.DayOfWeek + 7) % 7;
            return termStart.AddDays(daysUntil);
        }
    }
}
