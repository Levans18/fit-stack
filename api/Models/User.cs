using System.ComponentModel.DataAnnotations;

namespace FitStack.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        [Required]
        public string PasswordHash { get; set; } = null!;
    }
}