using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CengReservation.Models;

namespace CengReservation.Pages.Admin.ClassroomManager
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Classroom> Classroom { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Classroom = await _context.Classrooms.ToListAsync();
        }
    }
}
