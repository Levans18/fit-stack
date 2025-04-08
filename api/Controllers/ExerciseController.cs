using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitStack.API.Data;
using FitStack.API.DTOs;
using FitStack.API.Models;
using FitStack.API.Services;

namespace FitStack.API.Controllers
{
    [ApiController]
    [Route("exercises")]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CurrentUserService _currentUserService;

        public ExerciseController(AppDbContext context, CurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

       [HttpPost]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseDto dto)
        {
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) return Unauthorized(error);

            var workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.Id == dto.WorkoutId && w.UserId == user.Id);

            if (workout == null)
                return NotFound("Workout not found.");

            var exercise = new Exercise
            {
                Name = dto.Name,
                Sets = dto.Sets,
                Reps = dto.Reps,
                Weight = dto.Weight,
                WorkoutId = dto.WorkoutId
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Exercise added successfully",
                exercise = new
                {
                    id = exercise.Id,
                    name = exercise.Name,
                    sets = exercise.Sets,
                    reps = exercise.Reps,
                    weight = exercise.Weight,
                    workoutId = exercise.WorkoutId
                }
            });
        }

         [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetExerciseById(int exerciseId)
        {   
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) return Unauthorized(error);

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.Workout.UserId == user.Id);

            if (exercise == null)
                return NotFound("Exercise not found.");

            return Ok(new
            {
                id = exercise.Id,
                name = exercise.Name,
                sets = exercise.Sets,
                reps = exercise.Reps,
                weight = exercise.Weight,
                workoutId = exercise.WorkoutId,
            });
        }

        [HttpPut("{exerciseId}")]
        public async Task<IActionResult> UpdateExercise(int exerciseId, [FromBody] CreateExerciseDto dto)
        {
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) return Unauthorized(error);

            var exercise = await _context.Exercises
                .Include(e => e.Workout)
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.Workout.UserId == user.Id);

            if (exercise == null)
                return NotFound("Exercise not found.");

            exercise.Name = dto.Name;
            exercise.Sets = dto.Sets;
            exercise.Reps = dto.Reps;
            exercise.Weight = dto.Weight;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Exercise updated successfully." });
        }

        [HttpDelete("{exerciseId}")]
        public async Task<IActionResult> DeleteExercise(int exerciseId)
        {
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) return Unauthorized(error);

            var exercise = await _context.Exercises
                .Include(e => e.Workout)
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.Workout.UserId == user.Id);

            if (exercise == null)
                return NotFound("Exercise not found.");

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Exercise deleted successfully." });
        }
    }
}
