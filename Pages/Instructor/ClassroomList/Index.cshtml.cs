using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CengReservation.Data;
using CengReservation.Models;
using Microsoft.AspNetCore.Identity;

namespace CengReservation.Pages.Instructor.ClassroomList
{
    [Authorize(Roles = "Instructor")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Classroom> Classrooms { get; set; } = new();

        [BindProperty]
        public string Comment { get; set; } = string.Empty;

        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public int ClassroomId { get; set; }

        public async Task OnGetAsync()
        {
            Classrooms = await _context.Classrooms.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IActionResult> OnPostSendFeedbackAsync()
        {
            if (string.IsNullOrWhiteSpace(Comment))
            {
                ModelState.AddModelError(string.Empty, "Yorum boş olamaz.");
                await OnGetAsync();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var classroom = await _context.Classrooms.FindAsync(ClassroomId);
            if (classroom == null)
            {
                return NotFound();
            }

            var feedback = new Feedback
            {
                ClassroomId = ClassroomId,
                Comment = Comment,
                Rating = Rating,
                UserId = user.Id,
                SubmittedAt = DateTime.Now
            };

            try
            {
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Feedback kaydedilirken bir hata oluştu.");
                await OnGetAsync();
                return Page();
            }

            return RedirectToPage();
        }
    }
}
