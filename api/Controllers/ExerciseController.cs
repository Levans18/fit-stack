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
        private readonly ICurrentUserService _currentUserService;
        private readonly IExerciseService _exerciseService;

        public ExerciseController(AppDbContext context, CurrentUserService currentUserService, IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
            _currentUserService = currentUserService;
        }

       [HttpPost]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseDto dto)
        {
            try
            {
                // Validate the DTO
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (user, error) = await _currentUserService.GetAsync();
                if (user == null) return Unauthorized(error);

                var workout = await _exerciseService.CreateExerciseAsync(dto, user);
                
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

         [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetExerciseById(int exerciseId)
        {   
            try
            {
                var (user, error) = await _currentUserService.GetAsync();
                if (user == null)
                    return Unauthorized(error);

                var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId, user);
                return Ok(exercise);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
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
