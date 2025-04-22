public class WorkoutCompletionDto
{
    public int? Id { get; set; } // ID of the workout completion (useful for updates)
    public int WorkoutId { get; set; } // ID of the associated workout
    public string? Notes { get; set; } // Overall workout notes
    public DateTime? CompletedAt { get; set; } // Timestamp of when the workout was completed
    public List<CompletedExerciseDto> CompletedExercises { get; set; } = new List<CompletedExerciseDto>(); // List of completed exercises
}