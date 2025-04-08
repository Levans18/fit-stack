using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using FitStack.API.Data;
using FitStack.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using FitStack.API.Services;
using Microsoft.EntityFrameworkCore;
using FitStack.API.DTOs;

namespace FitStack.API.Controllers 
{

    [ApiController]
    [Route("workouts")]
    [Authorize]
    public class WorkoutController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CurrentUserService _currentUserService;
        private readonly IWorkoutService _workoutService;

        public WorkoutController(AppDbContext context, CurrentUserService currentUserService, IWorkoutService workoutService){
            _context = context;
            _currentUserService = currentUserService;
            _workoutService = workoutService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkouts()
        {
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) 
                return Unauthorized(error);
            
            var workouts = await _workoutService.GetWorkoutsForUserAsync(user) ?? new List<WorkoutResponseDto>();

            var now = DateTime.UtcNow;
            var pastWorkouts = workouts.Where(w => w.Date < now).OrderByDescending(w => w.Date).ToList();
            var upcomingWorkouts = workouts.Where(w => w.Date >= now).OrderBy(w => w.Date).ToList();
            
            return Ok(new
            {
                pastWorkouts,
                upcomingWorkouts
            });
        }

       [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById(int id)
        {   
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) return Unauthorized(error);

            var workout = await _workoutService.GetWorkoutByIdAsync(id, user); 

            if (workout == null)
                return NotFound("Workout not found.");

            return Ok(workout);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkout([FromBody] CreateWorkoutDto createWorkoutDto)
        {
            var (user, error) = await _currentUserService.GetAsync();

            if (user == null)
                return Unauthorized(error);
            
            var workout = new Workout
            {
                Name = createWorkoutDto.Name,
                Date = createWorkoutDto.Date,
                UserId = user.Id,
            };

            _context.Add(workout);
            await _context.SaveChangesAsync(); // <--- This line is crucial

            return Ok(new{
                message = "Successfully Created The Workout",
                workout = new {
                    id = workout.Id,
                    name = workout.Name,
                    date = workout.Date,
                }
            });
        }

       [HttpPost("{workoutId}/exercises")]
        public async Task<IActionResult> AddExerciseToWorkout(int workoutId, [FromBody] CreateExerciseDto dto)
        {
            Console.WriteLine("AddExerciseToWorkout called."); // Debugging line

            var (user, error) = await _currentUserService.GetAsync();
            if (user == null)
            {
                Console.WriteLine($"Unauthorized: {error}");
                return Unauthorized(error);
            }

            Console.WriteLine($"User ID: {user.Id}");
            var workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.UserId == user.Id);

            if (workout == null)
                return NotFound("Workout not found.");

            var exercise = new Exercise
            {
                Name = dto.Name,
                Sets = dto.Sets,
                Reps = dto.Reps,
                Weight = dto.Weight,
                WorkoutId = workoutId
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) return Unauthorized(error);

            var workout = await _workoutService.GetWorkoutByIdAsync(id, user); 

            if (workout == null)
                return NotFound("Workout not found.");

            await _workoutService.DeleteWorkoutAsync(id, user); 

            return Ok(new { message = "Successfully deleted the workout." });
        }
    }
}