
namespace FitStack.API.DTOs
{
    public class ExerciseResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public float Weight { get; set; }
        public int WorkoutId { get; set; }
    }
}