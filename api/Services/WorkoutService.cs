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
            Exercises = w.Exercises.Select(e => new ExerciseResponseDto
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
            Exercises = workout.Exercises.Select(e => new ExerciseResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Sets = e.Sets,
                Reps = e.Reps,
                Weight = e.Weight
            }).ToList()
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

    public async Task<bool> DeleteWorkoutAsync(int id, User user)
    {
        var workout = await _workoutRepository.GetByIdAsync(id);
        if (workout == null || workout.UserId != user.Id)
            return false;

        await _workoutRepository.DeleteAsync(id);
        return true;
    }
}