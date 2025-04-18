
using FitStack.API.DTOs;
using FitStack.API.Models;
using FitStack.API.Repositories;
using FitStack.API.Services;
using FitStack.Repositories;
using Moq;

namespace FitStackTests.API.Services {

    public class ExerciseServiceTests
    {
        private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;
        private readonly Mock<IWorkoutRepository> _workoutRepositoryMock;
        private readonly ExerciseService _exerciseService;

        public ExerciseServiceTests()
        {
            _exerciseRepositoryMock = new Mock<IExerciseRepository>();
            _workoutRepositoryMock = new Mock<IWorkoutRepository>();
            _exerciseService = new ExerciseService(_exerciseRepositoryMock.Object, _workoutRepositoryMock.Object);
        }

       [Fact]
        public async Task GetExerciseByIdAsync_ShouldReturnExercise_WhenExerciseExists()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            var mockExercise = new Exercise
            {
                Id = 1,
                Name = "Exercise A",
                Sets = 3,
                Reps = 10,
                Weight = 100,
                WorkoutId = 1
            };
            var mockWorkout = new Workout
            {
                Id = 1,
                UserId = mockUser.Id // User owns the workout
            };


            _workoutRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(mockWorkout);
            _exerciseRepositoryMock.Setup(repo => repo.GetExerciseByIdAsync(1)).ReturnsAsync(mockExercise);

            // Act
            var result = await _exerciseService.GetExerciseByIdAsync(1, mockUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ExerciseDto>(result);
            Assert.Equal(mockExercise.Id, result.Id);
            Assert.Equal(mockExercise.Name, result.Name);
            Assert.Equal(mockExercise.Sets, result.Sets);
            Assert.Equal(mockExercise.Reps, result.Reps);
            Assert.Equal(mockExercise.Weight, result.Weight);
            Assert.Equal(mockExercise.WorkoutId, result.WorkoutId);
        }

        [Fact]
        public async Task GetExerciseByIdAsync_ShouldThrowKeyNotFoundException_WhenExerciseDoesNotExist()
        {
            // Arrange
            var mockUser = new User { Id = 1 };

            _exerciseRepositoryMock
                .Setup(repo => repo.GetExerciseByIdAsync(1))
                .ThrowsAsync(new KeyNotFoundException("Exercise not found."));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _exerciseService.GetExerciseByIdAsync(1, mockUser));
        }

        [Fact]
        public async Task GetExerciseByIdAsync_ShouldThrowUnauthorizedAccessException_WhenUserDoesNotOwnWorkout()
        {
            // Arrange
            var mockUser = new User { Id = 1 }; // User making the request
           _exerciseRepositoryMock.Setup(repo => repo.GetExerciseByIdAsync(1)).ReturnsAsync(new Exercise
            {
                Id = 1,
                Name = "Exercise A",
                Sets = 3,
                Reps = 10,
                Weight = 100,
                WorkoutId = 1
            });

            _workoutRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new Workout
            {
                Id = 1,
                UserId = 2 // Different user to trigger UnauthorizedAccessException
            });
            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _exerciseService.GetExerciseByIdAsync(1, mockUser));
            Assert.Equal("You do not have permission to access this exercise.", exception.Message);
        }

        [Fact]
        public async Task UpdateExerciseAsync_ShouldUpdateExercise_WhenValid()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            var mockExercise = new Exercise
            {
                Id = 1,
                Name = "Old Exercise",
                Sets = 3,
                Reps = 10,
                Weight = 100,
                WorkoutId = 1
            };
            var mockWorkout = new Workout
            {
                Id = 1,
                UserId = mockUser.Id
            };
            var updateDto = new ExerciseDto
            {
                Name = "Updated Exercise",
                Sets = 4,
                Reps = 12,
                Weight = 120,
                WorkoutId = 1
            };

            _exerciseRepositoryMock.Setup(repo => repo.GetExerciseByIdAsync(1)).ReturnsAsync(mockExercise);
            _workoutRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(mockWorkout);

            // Act
            var result = await _exerciseService.UpdateExerciseAsync(1, updateDto, mockUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Name, result.Name);
            Assert.Equal(updateDto.Sets, result.Sets);
            Assert.Equal(updateDto.Reps, result.Reps);
            Assert.Equal(updateDto.Weight, result.Weight);
        }

        [Fact]
        public async Task UpdateExerciseAsync_ShouldThrowUnauthorizedAccessException_WhenUserDoesNotOwnWorkout()
        {
            // Arrange
            var mockUser = new User { Id = 1 };
            var mockExercise = new Exercise
            {
                Id = 1,
                Name = "Exercise A",
                Sets = 3,
                Reps = 10,
                Weight = 100,
                WorkoutId = 1
            };
            var mockWorkout = new Workout
            {
                Id = 1,
                UserId = 2 // Different user
            };
            var updateDto = new ExerciseDto
            {
                Name = "Updated Exercise",
                Sets = 4,
                Reps = 12,
                Weight = 120,
                WorkoutId = 1
            };

            _exerciseRepositoryMock.Setup(repo => repo.GetExerciseByIdAsync(1)).ReturnsAsync(mockExercise);
            _workoutRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(mockWorkout);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _exerciseService.UpdateExerciseAsync(1, updateDto, mockUser));
        }
    }
}