using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FitStack.API.Services;
using FitStack.API.DTOs;

namespace FitStack.API.Controllers 
{

    [ApiController]
    [Route("workouts")]
    [Authorize]
    public class WorkoutController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService, ICurrentUserService currentUserService)
        {
            _workoutService = workoutService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkouts()
        {
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) 
                return Unauthorized(error);

            var workouts = await _workoutService.GetWorkoutsForUserAsync(user) ?? new List<WorkoutResponseDto>();

            return Ok(workouts);
        }

       [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById(int id)
        {   
            //Get the User
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) 
                return Unauthorized(error);

            //Try to get the workout
            var workout = await _workoutService.GetWorkoutByIdAsync(id, user); 
            if (workout == null)
                return NotFound("Workout not found.");

            return Ok(workout);
        }

       [HttpPost]
        public async Task<IActionResult> CreateWorkout([FromBody] CreateWorkoutDto createWorkoutDto)
        {
            // Validate the DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the User
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null)
                return Unauthorized(error);

            // Try to create the workout
            try
            {
                var workout = await _workoutService.CreateWorkoutAsync(createWorkoutDto, user);

                var workoutResponse = new WorkoutResponseDto
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

                return CreatedAtAction(nameof(GetWorkoutById), new { id = workout.Id }, workoutResponse);
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                return StatusCode(500, "An error occurred while creating the workout.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) return Unauthorized(error);

            var success = await _workoutService.DeleteWorkoutAsync(id, user); 

            if(!success)
                return NotFound("Workout not found or you do not have permission to delete it.");

            return NoContent();
        }
    }
}