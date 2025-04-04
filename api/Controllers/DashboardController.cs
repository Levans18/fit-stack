
using FitStack.API.DTOs;
using FitStack.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitStack.API.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public DashboardController(AppDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var (user, error) = await _currentUserService.GetAsync();
        if (user == null) return Unauthorized(error);

        var workouts = await _context.Workouts
            .Where(w => w.UserId == user.Id)
            .Include(w => w.Exercises)
            .OrderByDescending(w => w.Date)
            .ToListAsync();

        var totalExercises = workouts.SelectMany(w => w.Exercises).Count();
        var totalWorkouts = workouts.Count;
        var lastWorkout = workouts.FirstOrDefault();
        var thisWeekWorkouts = workouts.Where(w => w.Date >= DateTime.UtcNow.AddDays(-7)).ToList();

        return Ok(new
        {
            totalExercises,
            totalWorkouts,
            thisWeekWorkouts = thisWeekWorkouts.Select(w => new
            {
                w.Id,
                w.Name,
                w.Date,
                exercises = w.Exercises.Select(e => new
                {
                    e.Name,
                    e.Sets,
                    e.Reps,
                    e.Weight
                })
            }),
            lastWorkout = lastWorkout != null ? new {
                date = lastWorkout.Date,
                exercises = lastWorkout.Exercises.Select(e => new {
                    e.Name,
                    e.Sets,
                    e.Reps,
                    e.Weight
                })
            } : null
        });
    }
}
