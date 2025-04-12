using System.ComponentModel.DataAnnotations;

namespace FitStack.API.Models{
    public class Workout
    {
        public int Id {get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public WorkoutCompletion? Completion { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}