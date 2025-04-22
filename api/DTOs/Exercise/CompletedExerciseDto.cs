public class CompletedExerciseDto
{
    public int Id { get; set; } // ID of the completed exercise (useful for updates)
    public string Name { get; set; } = null!; // Name of the exercise
    public string? Notes { get; set; } // Optional notes for the exercise
    public List<CompletedSetDto> CompletedSets { get; set; } = new List<CompletedSetDto>(); // List of completed sets
}