using System.ComponentModel.DataAnnotations;

namespace FitStack.API.Models{

    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int Sets { get; set; }
        public int Reps { get; set; }
        public float Weight { get; set; }

        public int WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;
    }
}