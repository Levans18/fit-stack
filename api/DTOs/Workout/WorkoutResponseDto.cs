
namespace FitStack.API.DTOs
{
    public class WorkoutResponseDto
    {
        public int Id { get; set; }
        public string Name {get; set; } = null!;
        public DateTime Date { get; set; }
        public List<ExerciseDto> Exercises { get; set; } = new List<ExerciseDto>();
    }
}