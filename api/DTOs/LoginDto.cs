using System.ComponentModel.DataAnnotations;

namespace FitStack.API.DTOs
{
    public class LoginDto
    {
        public string UsernameOrEmail { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}