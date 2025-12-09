using System.ComponentModel.DataAnnotations;

namespace JWTRefreshToken.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string position { get; set; } // Optional, can be null if not provided
    }
}
