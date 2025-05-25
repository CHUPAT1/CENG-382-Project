using Microsoft.AspNetCore.Identity;
using CengReservation.Models;


namespace CengReservation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}