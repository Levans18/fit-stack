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

        public WorkoutController(AppDbContext context, CurrentUserService currentUserService){
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkouts()
        {
            var (user, error) = await _currentUserService.GetAsync();

            if (user == null)
                return Unauthorized(error);
            
            var workouts = await _context.Workouts
                    .Where(w => w.UserId == user.Id)
                    .OrderByDescending(w => w.Date)
                    .Select(w => new WorkoutResponseDto
                    {
                        Id = w.Id,
                        Name = w.Name,
                        Date = w.Date
                    })
                    .ToListAsync();
            
            return Ok(workouts);
        }

       [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById(int id)
        {   
            var (user, error) = await _currentUserService.GetAsync();
            if (user == null) return Unauthorized(error);

            var workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == user.Id);

            if (workout == null)
                return NotFound("Workout not found.");

            return Ok(new
            {
                id = workout.Id,
                name = workout.Name,
                date = workout.Date,
                exercises = workout.Exercises.Select(e => new
                {
                    id = e.Id,
                    name = e.Name,
                    sets = e.Sets,
                    reps = e.Reps,
                    weight = e.Weight
                })
            });
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