using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitStack.API.Data;
using FitStack.API.Models;
using FitStack.API.DTOs;
using FitStack.API.Services;
using FitStack.API.Repositories;
using Moq;
using Xunit;

namespace api.tests.Services
{
    public class WorkoutServiceTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepositoryMock;
        private readonly WorkoutService _workoutService;

        public WorkoutServiceTests()
        {
            _workoutRepositoryMock = new Mock<IWorkoutRepository>();
            _workoutService = new WorkoutService(_workoutRepositoryMock.Object);
        }

        [Fact]
        public async Task GetWorkoutsForUserAsync_ShouldReturnListOfWorkouts()
        {
            // Arrange
            var mockUser = new User { Id = 1};
            var mockWorkouts = new List<Workout>
            {
                new Workout { Id = 1, Name = "Workout A", UserId = mockUser.Id },
                new Workout { Id = 2, Name = "Workout B", UserId = mockUser.Id }
            };

            _workoutRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockWorkouts);

            // Act
            var result = await _workoutService.GetWorkoutsForUserAsync(mockUser);
        
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<WorkoutResponseDto>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Workout A", result[0].Name);
            Assert.Equal("Workout B", result[1].Name);
        }

       [Fact]
        public async Task GetWorkoutByIdAsync_ShouldReturnWorkout_WhenWorkoutExists()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            var mockWorkout = new Workout
            {
                Id = 1,
                Name = "Workout A",
                UserId = 1,
                Exercises = new List<Exercise>
                {
                    new Exercise { Id = 1, Name = "Squats", Sets = 3, Reps = 10, Weight = 100 }
                }
            };

            _workoutRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(mockWorkout);

            // Act
            var result = await _workoutService.GetWorkoutByIdAsync(1, mockUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Workout A", result.Name);
        }

        [Fact]
        public async Task CreateWorkoutAsync_ShouldAddWorkoutSuccessfully()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            var newWorkoutDto = new CreateWorkoutDto { Name = "Workout C", Date = DateTime.UtcNow };
            var newWorkout = new Workout { Id = 3, Name = "Workout C", UserId = 1 };

            _workoutRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Workout>())).Returns(Task.CompletedTask);

            // Act
            var result = await _workoutService.CreateWorkoutAsync(newWorkoutDto, mockUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Workout C", result.Name);
        }
    }
}