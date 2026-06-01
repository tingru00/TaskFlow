using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute; // NSubstitute enligt uppgiftskravet
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Services;
using TaskFlow.Domain.Entities;
using Xunit;

namespace TaskFlow.Tests.Services;

public class TaskItemServiceTests
{
    private readonly ITaskItemRepository _repositoryMock;
    private readonly TaskItemService _sut; // System Under Test

    public TaskItemServiceTests()
    {
        // En mock av ditt TaskItem-repository
        _repositoryMock = Substitute.For<ITaskItemRepository>();

        // Injektera mocken i din TaskItemService
        _sut = new TaskItemService(_repositoryMock);
    }

    // --- GET TESTS ---

    [Fact]
    public async Task GetAllTasksAsync_HappyPath_ReturnsAllTasks()
    {
        // Arrange
        var fakeTasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Clean room" },
            new TaskItem { Id = 2, Title = "Buy groceries" }
        };
        _repositoryMock.GetAllAsync().Returns(Task.FromResult<IEnumerable<TaskItem>>(fakeTasks));

        // Act
        var result = await _sut.GetAllTasksAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, ((List<TaskItem>)result).Count);
    }

    [Fact]
    public async Task GetTaskByIdAsync_HappyPath_ReturnsTask()
    {
        // Arrange
        var expectedTask = new TaskItem { Id = 1, Title = "Clean room" };
        _repositoryMock.GetByIdAsync(1).Returns(Task.FromResult<TaskItem?>(expectedTask));

        // Act
        var result = await _sut.GetTaskByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Clean room", result.Title);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetTaskByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        _repositoryMock.GetByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<TaskItem?>(null));

        // Act
        var result = await _sut.GetTaskByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    // --- CREATE TESTS ---

    [Fact]
    public async Task CreateTaskAsync_HappyPath_CallsAddAndSaveChanges()
    {
        // Arrange
        var newTask = new TaskItem { Id = 1, Title = "Study C#" };

        // Act
        await _sut.CreateTaskAsync(newTask);

        // Assert
        await _repositoryMock.Received(1).AddAsync(newTask);
        await _repositoryMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CreateTaskAsync_WhenTaskIsNull_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _sut.CreateTaskAsync(null!);
        });
    }

    // --- UPDATE TESTS ---

    [Fact]
    public async Task UpdateTaskAsync_HappyPath_CallsUpdateAndSaveChanges()
    {
        // Arrange
        var existingTask = new TaskItem { Id = 1, Title = "Old Title" };

        // Act
        await _sut.UpdateTaskAsync(existingTask);

        // Assert
        _repositoryMock.Received(1).Update(existingTask);
        await _repositoryMock.Received(1).SaveChangesAsync();
    }

    // --- DELETE TESTS ---

    [Fact]
    public async Task DeleteTaskAsync_HappyPath_DeletesTaskWhenItExists()
    {
        // Arrange
        var taskToDelete = new TaskItem { Id = 1, Title = "To Delete" };
        _repositoryMock.GetByIdAsync(1).Returns(Task.FromResult<TaskItem?>(taskToDelete));

        // Act
        await _sut.DeleteTaskAsync(1);

        // Assert
        _repositoryMock.Received(1).Delete(taskToDelete);
        await _repositoryMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenObjectIsMissing_DoesNotCallDeleteOrSaveChanges()
    {
        // Arrange
        _repositoryMock.GetByIdAsync(999).Returns(Task.FromResult<TaskItem?>(null));

        // Act
        await _sut.DeleteTaskAsync(999);

        // Assert
        _repositoryMock.DidNotReceive().Delete(Arg.Any<TaskItem>());
        await _repositoryMock.DidNotReceive().SaveChangesAsync();
    }
}