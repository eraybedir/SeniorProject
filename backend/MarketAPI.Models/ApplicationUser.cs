using Microsoft.AspNetCore.Identity;

namespace MarketAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirebaseUid { get; set; }
        public required string Email { get; set; }
    }
}
