public class CompletedExercise
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Notes { get; set; } // exercise-specific notes

    public int WorkoutCompletionId { get; set; }
    public WorkoutCompletion WorkoutCompletion { get; set; } = null!;

    public ICollection<CompletedSet> CompletedSets { get; set; } = new List<CompletedSet>();
}