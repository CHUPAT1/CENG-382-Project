using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CengReservation.Models;

namespace CengReservation.Pages.Admin.InstructorManager
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public IList<ApplicationUser> Instructors { get; private set; } = new List<ApplicationUser>();


        [BindProperty]
        public NewInstructorInput NewInstructor { get; set; } = new NewInstructorInput();

        public class NewInstructorInput
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }


        public async Task OnGetAsync()
        {
            var allInstructors = await _userManager.GetUsersInRoleAsync("instructor");
            Instructors = allInstructors
                .Where(u => u.IsActive)
                .ToList();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = NewInstructor.Email,
                Email = NewInstructor.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, NewInstructor.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            await _userManager.AddToRoleAsync(user, "instructor");

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeactivateAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToPage();
        }
    }
}
