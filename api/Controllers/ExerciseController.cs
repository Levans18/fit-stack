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

        public ExerciseController(ICurrentUserService currentUserService, IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
            _currentUserService = currentUserService;
        }

       [HttpPost]
        public async Task<IActionResult> CreateExercise([FromBody] ExerciseDto dto)
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
        
       [HttpPost("{exerciseId}/complete")]
        public async Task<IActionResult> CompleteExercise(int exerciseId)
        {
            try
            {
                var (user, error) = await _currentUserService.GetAsync();
                if (user == null) return Unauthorized(error);

                var completedExercise = await _exerciseService.CompleteExerciseAsync(exerciseId, user);
                return Ok(completedExercise);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
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
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // Return 403 Forbidden
            }
        }

        [HttpPut("{exerciseId}")]
        public async Task<IActionResult> UpdateExercise(int exerciseId, [FromBody] ExerciseDto dto)
        {
            try
            {
                // Validate the DTO
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (user, error) = await _currentUserService.GetAsync();
                if (user == null) return Unauthorized(error);

                var exercise = await _exerciseService.UpdateExerciseAsync(exerciseId, dto, user);

                return Ok(exercise);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // Return 403 Forbidden
            }
        }

        [HttpDelete("{exerciseId}")]
        public async Task<IActionResult> DeleteExercise(int exerciseId)
        {
            try{
                // Validate the DTO
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (user, error) = await _currentUserService.GetAsync();
                if (user == null) return Unauthorized(error);

                var exercise = await _exerciseService.DeleteExerciseAsync(exerciseId, user);

                return Ok(new { message = "Exercise deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
