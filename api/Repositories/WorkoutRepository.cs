using FitStack.API.Data;
using FitStack.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitStack.API.Repositories
{
    public interface IWorkoutRepository
    {
        Task<List<Workout>> GetAllAsync();
        Task<Workout> GetByIdAsync(int id);
        Task AddAsync(Workout workout);
        Task DeleteAsync(int id);
        Task<WorkoutCompletion?> GetWorkoutCompletionByWorkoutIdAsync(int workoutId);
        Task AddWorkoutCompletionAsync(WorkoutCompletion workoutCompletion);
        Task UpdateWorkoutCompletionAsync(WorkoutCompletion workoutCompletion);
    }

    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly AppDbContext _context;

        public WorkoutRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Workout>> GetAllAsync()
        {
            return await _context.Workouts.ToListAsync();
        }

        public async Task<Workout> GetByIdAsync(int id)
        {
            var workout = await _context.Workouts
                .Include(w => w.Exercises) // Explicitly include Exercises
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workout == null)
                throw new KeyNotFoundException($"Workout with ID {id} was not found.");

            return workout;
        }

        public async Task AddAsync(Workout workout)
        {
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
            }
        }

        /*Workout Completion Handling*/
        public async Task<WorkoutCompletion?> GetWorkoutCompletionByWorkoutIdAsync(int workoutId)
        {
            return await _context.WorkoutCompletions
                .Include(wc => wc.CompletedExercises)
                .ThenInclude(ce => ce.CompletedSets)
                .FirstOrDefaultAsync(wc => wc.WorkoutId == workoutId);
        }

        public async Task AddWorkoutCompletionAsync(WorkoutCompletion workoutCompletion)
        {
            await _context.WorkoutCompletions.AddAsync(workoutCompletion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWorkoutCompletionAsync(WorkoutCompletion workoutCompletion)
        {
            _context.WorkoutCompletions.Update(workoutCompletion);
            await _context.SaveChangesAsync();
        }
    }
}