using Microsoft.AspNetCore.Identity;

namespace CryptoTracker.API.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string Id { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
