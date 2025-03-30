
using System.ComponentModel.DataAnnotations;

namespace FitStack.API.DTOs
{
    public class CreateWorkoutDto
    {
            [Required]
            public string Name { get; set; } = null!;

            public DateTime Date { get; set; } = DateTime.UtcNow; 
    }
}