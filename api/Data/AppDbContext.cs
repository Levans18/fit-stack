using FitStack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FitStack.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Workout> Workouts => Set<Workout>();
        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<WorkoutCompletion> WorkoutCompletions => Set<WorkoutCompletion>();
        public DbSet<CompletedExercise> CompletedExercises => Set<CompletedExercise>();
        public DbSet<CompletedSet> CompletedSets => Set<CompletedSet>();
    }
}