using FitStack.API.Models;

public class Set
{
    public int Id { get; set; }

    public int Reps { get; set; }
    public float Weight { get; set; }

    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
}