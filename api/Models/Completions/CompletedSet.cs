public class CompletedSet
{
    public int Id { get; set; }

    public int Reps { get; set; }
    public float Weight { get; set; }

    public int CompletedExerciseId { get; set; }
    public CompletedExercise CompletedExercise { get; set; } = null!;
}