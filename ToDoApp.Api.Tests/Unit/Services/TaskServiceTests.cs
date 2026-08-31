using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Common;
using ToDoApp.Api.Data;
using ToDoApp.Api.Models;
using ToDoApp.Api.Services.Implementations;
using ToDoApp.Api.Tests.Helpers;

namespace ToDoApp.Api.Tests.Unit.Services
{
    public class TaskServiceTests
    {
        private readonly AppDbContext _database;
        private readonly TaskService _sut;

        public TaskServiceTests()
        {
            _database = InMemoryDbContextFactory.Create();
            _sut = new TaskService(_database);
        }

        private async Task<TaskItem> CreateExistingTaskAsync()
        {
            var task = new TaskItem { UserId = Guid.NewGuid(), TaskDescription = "original description" };

            await _database.Tasks.AddAsync(task);
            await _database.SaveChangesAsync();

            return task;
        }

        [Fact]
        public async Task GetTasks_UserHasTasks_ReturnsOnlyTheirTasks()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            await _database.Tasks.AddAsync(new TaskItem { TaskDescription = "user1 task", UserId = userId1 });
            await _database.Tasks.AddAsync(new TaskItem { TaskDescription = "user2 task", UserId = userId2 });
            await _database.SaveChangesAsync();

            //Act
            var actual = await _sut.GetTasks(userId1);

            //Assert
            Assert.Multiple(
                () => Assert.All(actual.Data!, t => Assert.Equal(userId1, t.UserId)),
                () => Assert.Single(actual.Data!)
                );
        }

        [Fact]
        public async Task PostTask_ValidDescription_ReturnsSuccess()
        {
            // Arrange
            const string description = "test description";

            //Act
            var actual = await _sut.PostTask(new TaskItem { TaskDescription = description });

            //Assert
            var savedTask = Assert.Single(await _database.Tasks.ToListAsync());

            Assert.Multiple(
                () => Assert.True(actual.Success),
                () => Assert.Equal(description, actual.Data!.TaskDescription),
                () => Assert.Equal(description, savedTask.TaskDescription),
                () => Assert.False(savedTask.IsCompleted)
                );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task PostTask_InvalidDescription_ReturnsFailure(string description)
        {
            //Act
            var actual = await _sut.PostTask(new TaskItem { TaskDescription = description });

            //Assert
            Assert.Multiple(
                () => Assert.False(actual.Success),
                () => Assert.Equal(ServiceErrorType.ValidationFailed, actual.ErrorType),
                () => Assert.Empty(_database.Tasks.ToList())
                );
        }

        [Fact]
        public async Task ModifyTask_ValidUpdate_ReturnsSuccess()
        {
            //Arrange
            var existingTask = await CreateExistingTaskAsync();
            const string updatedDescription = "updated";

            //Act
            var actual = await _sut.ModifyTask(existingTask.TaskItemId, new TaskItem { TaskDescription = updatedDescription });

            //Assert
            Assert.Multiple(
                () => Assert.True(actual.Success),
                () => Assert.Equal(updatedDescription, actual.Data!.TaskDescription)
                );
        }

        [Fact]
        public async Task ModifyTask_TaskNotFound_ReturnsFailure()
        {
            //Arrange
            var existingTask = await CreateExistingTaskAsync();
            var nonExistentId = existingTask.TaskItemId + 1;
            const string updatedDescription = "updated";

            //Act
            var actual = await _sut.ModifyTask(nonExistentId, new TaskItem { TaskDescription = updatedDescription });

            //Assert
            var task = await _database.Tasks.FindAsync(existingTask.TaskItemId);

            Assert.Multiple(
                () => Assert.False(actual.Success),
                () => Assert.Equal(ServiceErrorType.NotFound, actual.ErrorType),
                () => Assert.Equal(existingTask.TaskDescription, task!.TaskDescription)
                );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ModifyTask_InvalidDescription_ReturnsFailure(string description)
        {
            //Arrange
            var existingTask = await CreateExistingTaskAsync();

            //Act
            var actual = await _sut.ModifyTask(existingTask.TaskItemId, new TaskItem { TaskDescription = description });

            //Assert
            var task = await _database.Tasks.FindAsync(existingTask.TaskItemId);

            Assert.Multiple(
                () => Assert.False(actual.Success),
                () => Assert.Equal(ServiceErrorType.ValidationFailed, actual.ErrorType),
                () => Assert.Equal(existingTask.TaskDescription, task!.TaskDescription)
                );
        }

        [Fact]
        public async Task DeleteTask_TaskFound_ReturnsSuccess()
        {
            //Arrange
            var existingTask = await CreateExistingTaskAsync();

            //Act
            var actual = await _sut.DeleteTask(existingTask.TaskItemId);

            //Assert
            Assert.Multiple(
                () => Assert.True(actual.Success),
                () => Assert.Empty(_database.Tasks.ToList())
                );
        }

        [Fact]
        public async Task DeleteTask_TaskNotFound_ReturnsFailure()
        {
            //Arrange
            var existingTask = await CreateExistingTaskAsync();
            var nonExistentId = existingTask.TaskItemId + 1;

            //Act
            var actual = await _sut.DeleteTask(nonExistentId);

            //Assert
            Assert.Multiple(
                () => Assert.False(actual.Success),
                () => Assert.Equal(ServiceErrorType.NotFound, actual.ErrorType),
                () => Assert.Single(_database.Tasks.ToList())
                );
        }
    }
}
