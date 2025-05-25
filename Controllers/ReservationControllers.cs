using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CengReservation.Data;

namespace CengProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Classroom)
                .Select(r => new
                {
                    id = r.Id,
                    title = r.Classroom.Name,
                    start = r.StartTime,
                    end = r.EndTime,
                    classroomId = r.ClassroomId
                })
                .ToListAsync();

            return Ok(reservations);
        }
    }
}