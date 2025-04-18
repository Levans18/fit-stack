using FitStack.API.DTOs;
using FitStack.API.Models;
using FitStack.API.Repositories;
using FitStack.Repositories;

namespace FitStack.API.Services
{
    public interface IExerciseService
    {
        Task<ExerciseDto> GetExerciseByIdAsync(int id, User user);
        Task<IEnumerable<ExerciseDto>> GetExercisesForWorkoutAsync(int workoutId, User user);
        Task<ExerciseDto> CreateExerciseAsync(ExerciseDto dto, User user);
        Task<ExerciseDto> UpdateExerciseAsync(int id, ExerciseDto dto, User user);
        Task<bool> DeleteExerciseAsync(int id, User user);
    }

    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IWorkoutRepository _workoutRepository;

        public ExerciseService(IExerciseRepository exerciseRepository, IWorkoutRepository workoutRepository)
        {
            _exerciseRepository = exerciseRepository;
            _workoutRepository = workoutRepository;
        }

        public async Task<ExerciseDto> GetExerciseByIdAsync(int id, User user)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);

            // Check if the exercise belongs to the user
            var workout = await _workoutRepository.GetByIdAsync(exercise.WorkoutId);
            if (workout.UserId != user.Id)
                throw new UnauthorizedAccessException("You do not have permission to access this exercise.");

            return new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                Weight = exercise.Weight,
                WorkoutId = exercise.WorkoutId
            };
        }

        public async Task<IEnumerable<ExerciseDto>> GetExercisesForWorkoutAsync(int workoutId, User user)
        {
            var exercises = await _exerciseRepository.GetExercisesByWorkoutIdAsync(workoutId);

            return exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                Sets = e.Sets,
                Reps = e.Reps,
                Weight = e.Weight,
                WorkoutId = e.WorkoutId
            });
        }

        public async Task<ExerciseDto> CreateExerciseAsync(ExerciseDto dto, User user)
        {
            // Validate the user and workout
            var workout = await _workoutRepository.GetByIdAsync(dto.WorkoutId);
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

            return new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                Weight = exercise.Weight,
                WorkoutId = exercise.WorkoutId
            };
        }

        public async Task<ExerciseDto> UpdateExerciseAsync(int id, ExerciseDto dto, User user)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);

            // Check if the exercise belongs to the user
            var workout = await _workoutRepository.GetByIdAsync(exercise.WorkoutId);
            if (workout.UserId != user.Id)
                throw new UnauthorizedAccessException("You do not have permission to modify this exercise.");

            exercise.Name = dto.Name;
            exercise.Sets = dto.Sets;
            exercise.Reps = dto.Reps;
            exercise.Weight = dto.Weight;

            await _exerciseRepository.UpdateExerciseAsync(exercise);
            // Return the updated exercise as a DTO
            return new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Sets = exercise.Sets,
                Reps = exercise.Reps,
                Weight = exercise.Weight,
                WorkoutId = exercise.WorkoutId
            };
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