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
        Task<Workout?> GetByIdAsync(int id);
        Task AddAsync(Workout workout);
        Task DeleteAsync(int id);
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

        public async Task<Workout?> GetByIdAsync(int id)
        {
            return await _context.Workouts
                .Include(w => w.Exercises) // Explicitly include Exercises
                .FirstOrDefaultAsync(w => w.Id == id);
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
    }
}