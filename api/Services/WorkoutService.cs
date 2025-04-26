using FitStack.API.Data;
using FitStack.API.DTOs;
using FitStack.API.Models;
using FitStack.API.Repositories;
using Microsoft.EntityFrameworkCore;

public interface IWorkoutService
{
    Task<List<WorkoutResponseDto>> GetWorkoutsForUserAsync(User user);
    Task<WorkoutResponseDto?> GetWorkoutByIdAsync(int id, User user);
    Task<Workout> CreateWorkoutAsync(CreateWorkoutDto dto, User user);
    Task<bool> DeleteWorkoutAsync(int id, User user);
    Task<WorkoutCompletion?> UpdateWorkoutCompletionAsync(int workoutId, WorkoutCompletionDto dto, User user);
    Task<WorkoutCompletion?> GetWorkoutCompletionByWorkoutIdAsync(int workoutId, User user);
    Task<WorkoutCompletion?> AddWorkoutCompletionAsync(WorkoutCompletionDto dto, User user);
}

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;

    public WorkoutService(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<List<WorkoutResponseDto>> GetWorkoutsForUserAsync(User user)
    {
        // Retrieve all workouts from the repository
        var workouts = await _workoutRepository.GetAllAsync();

        // Filter workouts by the user's ID
        var userWorkouts = workouts.Where(w => w.UserId == user.Id);

        // Map the filtered workouts to WorkoutResponseDto objects
        return userWorkouts.Select(w => new WorkoutResponseDto
        {
            Id = w.Id,
            Name = w.Name,
            Date = w.Date,
            Exercises = w.Exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                Sets = e.Sets,
                Reps = e.Reps,
                Weight = e.Weight
            }).ToList()
        }).ToList();
    }

    public async Task<WorkoutResponseDto?> GetWorkoutByIdAsync(int id, User user)
    {
        var workout = await _workoutRepository.GetByIdAsync(id);
        if (workout == null || workout.UserId != user.Id)
            return null;

        return new WorkoutResponseDto
        {
            Id = workout.Id,
            Name = workout.Name,
            Date = workout.Date,
            Exercises = workout.Exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                Sets = e.Sets,
                Reps = e.Reps,
                Weight = e.Weight
            }).ToList(),
            Completion = workout.Completion == null ? null : new WorkoutCompletionDto
            {
                Id = workout.Completion.Id,
                CompletedAt = workout.Completion.CompletedAt,
                Notes = workout.Completion.Notes,
                CompletedExercises = workout.Completion.CompletedExercises.Select(ce => new CompletedExerciseDto
                {
                    Id = ce.Id,
                    Name = ce.Name,
                    Notes = ce.Notes,
                    CompletedSets = ce.CompletedSets.Select(cs => new CompletedSetDto
                    {
                        Id = cs.Id,
                        Reps = cs.Reps,
                        Weight = cs.Weight
                    }).ToList()
                }).ToList()
            }
        };
    }

    public async Task<Workout> CreateWorkoutAsync(CreateWorkoutDto dto, User user)
    {
        var workout = new Workout
        {
            Name = dto.Name,
            Date = dto.Date,
            UserId = user.Id
        };

        await _workoutRepository.AddAsync(workout);
        return workout;
    }

    public async Task<WorkoutCompletion?> AddWorkoutCompletionAsync(WorkoutCompletionDto dto, User user)
    {
        // Validate the DTO
        if (dto.CompletedExercises == null || !dto.CompletedExercises.Any())
            throw new ArgumentException("At least one completed exercise is required.");

        foreach (var exercise in dto.CompletedExercises)
        {
            if (string.IsNullOrWhiteSpace(exercise.Name))
                throw new ArgumentException("Exercise name cannot be null or empty.");

            if (exercise.CompletedSets == null || !exercise.CompletedSets.Any())
                throw new ArgumentException($"Exercise '{exercise.Name}' must have at least one completed set.");

            foreach (var set in exercise.CompletedSets)
            {
                if (set.Reps < 0)
                    throw new ArgumentException($"Set in exercise '{exercise.Name}' has invalid reps: {set.Reps}.");
                if (set.Weight < 0)
                    throw new ArgumentException($"Set in exercise '{exercise.Name}' has invalid weight: {set.Weight}.");
            }
        }
        
        // Step 1: Retrieve the workout
        var workout = await _workoutRepository.GetByIdAsync(dto.WorkoutId);
        if (workout == null || workout.UserId != user.Id)
            throw new UnauthorizedAccessException("You do not have permission to complete this workout.");

        // Step 2: Check if the workout already has a completion
        if (workout.Completion != null)
            throw new InvalidOperationException("This workout has already been completed.");

        // Step 3: Create a new WorkoutCompletion
        var workoutCompletion = new WorkoutCompletion
        {
            WorkoutId = workout.Id,
            CompletedAt = DateTime.UtcNow,
            Notes = dto.Notes,
            CompletedExercises = dto.CompletedExercises.Select(e => new CompletedExercise
            {
                Name = e.Name,
                Notes = e.Notes,
                CompletedSets = e.CompletedSets.Select(s => new CompletedSet
                {
                    Reps = s.Reps,
                    Weight = s.Weight
                }).ToList()
            }).ToList()
        };

        // Step 4: Save the WorkoutCompletion
        workout.Completion = workoutCompletion;
        await _workoutRepository.UpdateAsync(workout);

        return workoutCompletion;
    }

    public async Task<WorkoutCompletion?> UpdateWorkoutCompletionAsync(int workoutId, WorkoutCompletionDto dto, User user)
    {
        // Step 1: Retrieve the workout
        var workout = await _workoutRepository.GetByIdAsync(workoutId);
        if (workout == null || workout.UserId != user.Id)
            throw new UnauthorizedAccessException("You do not have permission to update this workout.");

        // Step 2: Check if the workout has a completion
        if (workout.Completion == null)
            throw new InvalidOperationException("This workout has not been completed yet.");

        // Step 3: Update the WorkoutCompletion
        workout.Completion.Notes = dto.Notes;
        workout.Completion.CompletedExercises = dto.CompletedExercises.Select(e => new CompletedExercise
        {
            Name = e.Name,
            Notes = e.Notes,
            CompletedSets = e.CompletedSets.Select(s => new CompletedSet
            {
                Reps = s.Reps,
                Weight = s.Weight
            }).ToList()
        }).ToList();

        // Step 4: Save the updated WorkoutCompletion
        await _workoutRepository.UpdateAsync(workout);

        return workout.Completion;
    }

    public async Task<WorkoutCompletion?> GetWorkoutCompletionByWorkoutIdAsync(int workoutId, User user)
    {
        // Step 1: Retrieve the workout
        var workout = await _workoutRepository.GetByIdAsync(workoutId);
        if (workout == null || workout.UserId != user.Id)
            throw new UnauthorizedAccessException("You do not have permission to access this workout.");

        // Step 2: Retrieve the WorkoutCompletion
        var workoutCompletion = await _workoutRepository.GetWorkoutCompletionByWorkoutIdAsync(workoutId);

        return workoutCompletion;
    }

    public async Task<bool> DeleteWorkoutAsync(int id, User user)
    {
        var workout = await _workoutRepository.GetByIdAsync(id);
        if (workout == null || workout.UserId != user.Id)
            return false;

        await _workoutRepository.DeleteAsync(id);
        return true;
    }
}