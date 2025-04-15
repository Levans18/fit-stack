using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FitStack.API.Services;
using FitStack.API.Controllers;
using FitStack.API.Models;
using FitStack.API.DTOs;
using FitStack.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitStack.API.Tests.Controllers
{
    public class WorkoutControllerTests
    {
        private readonly Mock<IWorkoutService> _mockWorkoutService;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly WorkoutController _controller;

        public WorkoutControllerTests()
        {
            _mockWorkoutService = new Mock<IWorkoutService>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _controller = new WorkoutController(_mockWorkoutService.Object, _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task GetWorkouts_ReturnsOkResult_WithListOfWorkouts()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            var workouts = new List<WorkoutResponseDto>
            {
                new WorkoutResponseDto { Id = 1, Name = "Workout A" },
                new WorkoutResponseDto { Id = 2, Name = "Workout B" }
            };

            _mockCurrentUserService
                .Setup(service => service.GetAsync())
                .ReturnsAsync((mockUser, null));

            _mockWorkoutService
                .Setup(service => service.GetWorkoutsForUserAsync(mockUser))
                .ReturnsAsync(workouts);

            // Act
            var result = await _controller.GetWorkouts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<WorkoutResponseDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
    public async Task GetWorkouts_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        var mockUser = new User { Id = 1 };
        var workouts = new List<WorkoutResponseDto>();

        _mockCurrentUserService
            .Setup(service => service.GetAsync())
            .ReturnsAsync((mockUser, null));

        _mockWorkoutService
            .Setup(service => service.GetWorkoutsForUserAsync(mockUser))
            .ReturnsAsync(workouts);

        // Act
        var result = await _controller.GetWorkouts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<WorkoutResponseDto>>(okResult.Value);
        Assert.Empty(returnValue);
    }

        [Fact]
        public async Task GetWorkoutById_ReturnsNotFound_WhenWorkoutDoesNotExist()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            int workoutId = 1;

            _mockCurrentUserService
                .Setup(service => service.GetAsync())
                .ReturnsAsync((mockUser, null));

            _mockWorkoutService
                .Setup(service => service.GetWorkoutByIdAsync(workoutId, mockUser))
                .ReturnsAsync((WorkoutResponseDto?)null);

            // Act
            var result = await _controller.GetWorkoutById(workoutId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Workout not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetWorkoutById_ReturnsOkResult_WithWorkout()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            int workoutId = 1;
            var workout = new WorkoutResponseDto { Id = workoutId, Name = "Workout A" };

            _mockCurrentUserService
                .Setup(service => service.GetAsync())
                .ReturnsAsync((mockUser, null));

            _mockWorkoutService
                .Setup(service => service.GetWorkoutByIdAsync(workoutId, mockUser))
                .ReturnsAsync(workout);

            // Act
            var result = await _controller.GetWorkoutById(workoutId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<WorkoutResponseDto>(okResult.Value);
            Assert.Equal(workoutId, returnValue.Id);
        }

        [Fact]
        public async Task CreateWorkout_ReturnsCreatedAtActionResult_WithWorkout()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            var createWorkoutDto = new CreateWorkoutDto { Name = "Workout A", Date = DateTime.UtcNow };
            var createdWorkout = new WorkoutResponseDto { Id = 1, Name = "Workout A" };

            _mockCurrentUserService
                .Setup(service => service.GetAsync())
                .ReturnsAsync((mockUser, null));

            _mockWorkoutService
                .Setup(service => service.CreateWorkoutAsync(createWorkoutDto, mockUser))
                .ReturnsAsync(new Workout { Id = createdWorkout.Id, Name = createdWorkout.Name });

            // Act
            var result = await _controller.CreateWorkout(createWorkoutDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<WorkoutResponseDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task DeleteWorkout_ReturnsNoContent_WhenWorkoutIsDeleted()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            int workoutId = 1;

            _mockCurrentUserService
                .Setup(service => service.GetAsync())
                .ReturnsAsync((mockUser, null));

            _mockWorkoutService
                .Setup(service => service.DeleteWorkoutAsync(workoutId, mockUser))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteWorkout(workoutId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteWorkout_ReturnsNotFound_WhenWorkoutDoesNotExist()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            int workoutId = 1;

            _mockCurrentUserService
                .Setup(service => service.GetAsync())
                .ReturnsAsync((mockUser, null));

            _mockWorkoutService
                .Setup(service => service.DeleteWorkoutAsync(workoutId, mockUser))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteWorkout(workoutId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Workout not found or you do not have permission to delete it.", notFoundResult.Value);
        }
    }
}