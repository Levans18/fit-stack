using System.ComponentModel.DataAnnotations;

namespace FitStack.API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username {get; set;} = null;

        [Required]
        [EmailAddress]
        public string Email {get; set;} = null;

        [Required]
        public string Password {get; set; } = null;
    }

}