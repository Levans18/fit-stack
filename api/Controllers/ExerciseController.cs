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

                var exercise = await _exerciseService.CreateExerciseAsync(dto, user);
                
                if (exercise == null)
                    return NotFound("Exercise Not Created.");

                return Ok(exercise);
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
            try
            {
                // Validate the DTO
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (user, error) = await _currentUserService.GetAsync();
                if (user == null) return Unauthorized(error);

                var exercise = await _exerciseService.UpdateExerciseAsync(exerciseId, dto, user);
                
                if (exercise == null)
                    return NotFound("Exercise not found.");

                return Ok(exercise);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
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
