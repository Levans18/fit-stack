
using System.ComponentModel.DataAnnotations;

namespace FitStack.API.DTOs
{
    public class CreateExerciseDto
    {
        [Required]
        public int WorkoutId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Range(1, 100)]
        public int Sets { get; set; }

        [Range(1, 100)]
        public int Reps { get; set; }

        [Range(0, 5000)]
        public float Weight { get; set; }
    }
}