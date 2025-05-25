using CengReservation.Data;
using CengReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CengReservation.Pages.Admin.ClassroomManager
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Classroom Classroom { get; set; } = default!;
        public List<Feedback> Feedbacks { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Classrooms == null)
                return NotFound();

            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (classroom == null)
                return NotFound();

            Classroom = classroom;

            Feedbacks = await _context.Feedbacks
                .Where(f => f.ClassroomId == id)
                .Include(f => f.User)
                .OrderByDescending(f => f.SubmittedAt)
                .ToListAsync();

            return Page();
        }
    }
}
