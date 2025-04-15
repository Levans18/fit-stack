using FitStack.API.DTOs;
using FitStack.API.Models;
using FitStack.API.Repositories;
using FitStack.Repositories;

namespace FitStack.API.Services
{
    public interface IExerciseService
    {
        Task<ExerciseResponseDto?> CreateExerciseAsync(CreateExerciseDto dto, User user);
        Task<ExerciseResponseDto?> GetExerciseByIdAsync(int id, User user);
        Task<IEnumerable<ExerciseResponseDto>> GetExercisesForWorkoutAsync(int workoutId, User user);
        Task<bool> DeleteExerciseAsync(int id, User user);
    }

    public class ExerciseService : IExerciseService
    {
        private readonly ExerciseRepository _exerciseRepository;
        private readonly WorkoutRepository _workoutRepository;

        public ExerciseService(ExerciseRepository exerciseRepository, WorkoutRepository workoutRepository)
        {
            _exerciseRepository = exerciseRepository;
            _workoutRepository = workoutRepository;
        }

        public async Task<ExerciseResponseDto?> CreateExerciseAsync(CreateExerciseDto dto, User user)
        {
            // Validate the user and workout
            var workout = await _workoutRepository.GetByIdAsync(dto.WorkoutId);
            if (workout == null) 
                throw new KeyNotFoundException("Workout not found.");

            if (workout.UserId != user.Id)
                throw new UnauthorizedAccessException("You do not have permission to modify this workout.");

            var exercise = new Exercise
            {
                Name = dto.Name,
                Sets = dto.Sets,
                Reps = dto.Reps,
                Weight = dto.Weight,
                WorkoutId = dto.WorkoutId
            };

            await _exerciseRepository.AddExerciseAsync(exercise);

            return new ExerciseResponseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                Weight = exercise.Weight,
                WorkoutId = exercise.WorkoutId
            };
        }

        public async Task<ExerciseResponseDto?> GetExerciseByIdAsync(int id, User user)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
            if (exercise == null) return null;

            return new ExerciseResponseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                Weight = exercise.Weight,
                WorkoutId = exercise.WorkoutId
            };
        }

        public async Task<IEnumerable<ExerciseResponseDto>> GetExercisesForWorkoutAsync(int workoutId, User user)
        {
            var exercises = await _exerciseRepository.GetExercisesByWorkoutIdAsync(workoutId);

            return exercises.Select(e => new ExerciseResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Sets = e.Sets,
                Reps = e.Reps,
                Weight = e.Weight,
                WorkoutId = e.WorkoutId
            });
        }

        public async Task<bool> DeleteExerciseAsync(int id, User user)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
            if (exercise == null) return false;

            // Check if the exercise belongs to the user
            var workout = await _workoutRepository.GetByIdAsync(exercise.WorkoutId);
            if (workout == null || workout.UserId != user.Id)
                throw new UnauthorizedAccessException("You do not have permission to delete this exercise.");

            await _exerciseRepository.DeleteExerciseAsync(id);
            return true;
        }
    }
}