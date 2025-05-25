using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CengReservation.Models;

namespace CengReservation.Pages.Admin.TermManager
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Term> Term { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Term = await _context.Terms.ToListAsync();
        }
    }
}
