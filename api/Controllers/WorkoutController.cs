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
            
            var workouts = await _workoutService.GetWorkoutsForUserAsync(user);
            if (workouts == null || workouts.Count == 0) 
                return NotFound("No workouts found.");
            
            return Ok(workouts);
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
    }
}