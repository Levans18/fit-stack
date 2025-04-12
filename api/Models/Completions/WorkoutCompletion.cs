using FitStack.API.Models;

public class WorkoutCompletion
{
    public int Id { get; set; }

    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    public string? Notes { get; set; } // overall workout notes

    public int WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;

    public ICollection<CompletedExercise> CompletedExercises { get; set; } = new List<CompletedExercise>();
}