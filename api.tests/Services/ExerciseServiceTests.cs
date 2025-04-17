
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

            _exerciseRepositoryMock.Setup(repo => repo.GetExerciseByIdAsync(1)).ReturnsAsync(mockExercise);

            // Act
            var result = await _exerciseService.GetExerciseByIdAsync(1, mockUser);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ExerciseResponseDto>(result);
            Assert.Equal(mockExercise.Id, result.Id);
            Assert.Equal(mockExercise.Name, result.Name);
            Assert.Equal(mockExercise.Sets, result.Sets);
            Assert.Equal(mockExercise.Reps, result.Reps);
            Assert.Equal(mockExercise.Weight, result.Weight);
            Assert.Equal(mockExercise.WorkoutId, result.WorkoutId);
        }

                [Fact]
        public async Task GetExerciseByIdAsync_ShouldKeyNotFound_WhenExerciseDoesntExist()
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

            _exerciseRepositoryMock.Setup(repo => repo.GetExerciseByIdAsync(1)).ReturnsAsync(null);

            // Act
            var result = await _exerciseService.GetExerciseByIdAsync(1, mockUser);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ExerciseResponseDto>(result);
            Assert.Equal(mockExercise.Id, result.Id);
            Assert.Equal(mockExercise.Name, result.Name);
            Assert.Equal(mockExercise.Sets, result.Sets);
            Assert.Equal(mockExercise.Reps, result.Reps);
            Assert.Equal(mockExercise.Weight, result.Weight);
            Assert.Equal(mockExercise.WorkoutId, result.WorkoutId);
        }
        
    }
}