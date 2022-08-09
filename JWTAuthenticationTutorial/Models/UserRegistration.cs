using System.ComponentModel.DataAnnotations;

namespace JWTAuthenticationTutorial.Models
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "User Name is mandatory.")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is mandatory.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is mandatory.")]
        public string? Password { get; set; }
    }
}
