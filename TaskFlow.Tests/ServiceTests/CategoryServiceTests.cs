using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute; 
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Services;
using TaskFlow.Domain.Entities;
using Xunit;

namespace TaskFlow.Tests;

public class CategoryServiceTests
{
    private readonly ICategoryRepository _repositoryMock;
    private readonly CategoryService _sut;

    public CategoryServiceTests()
    {
        // En mock med NSubstitute-syntax
        _repositoryMock = Substitute.For<ICategoryRepository>();

        // Injekteras i service
        _sut = new CategoryService(_repositoryMock);
    }

    // --- GET TESTS --- 

    [Fact]
    public async Task GetAllCategoriesAsync_HappyPath_ReturnsAllCategories()
    {
        // Arrange
        var fakeCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Work" },
            new Category { Id = 2, Name = "Personal" }
        };
        _repositoryMock.GetAllAsync().Returns(Task.FromResult<IEnumerable<Category>>(fakeCategories));

        // Act
        var result = await _sut.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, ((List<Category>)result).Count);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_HappyPath_ReturnsCategory()
    {
        // Arrange
        var expectedCategory = new Category { Id = 1, Name = "Work" };
        _repositoryMock.GetByIdAsync(1).Returns(Task.FromResult<Category?>(expectedCategory));

        // Act
        var result = await _sut.GetCategoryByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Work", result.Name);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        _repositoryMock.GetByIdAsync(Arg.Any<int>()).Returns(Task.FromResult<Category?>(null));

        // Act
        var result = await _sut.GetCategoryByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

   // --- CREATE TESTS ---

    [Fact]
    public async Task CreateCategoryAsync_HappyPath_CallsAddAndSaveChanges()
    {
        // Arrange
        var newCategory = new Category { Id = 1, Name = "Training" };

        // Act
        await _sut.CreateCategoryAsync(newCategory);

        // Assert
        // NSubstitute-verifiering av anrop
        await _repositoryMock.Received(1).AddAsync(newCategory);
        await _repositoryMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CreateCategoryAsync_WhenCategoryIsNull_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _sut.CreateCategoryAsync(null!);
        });
    }

    // -- UPDATE TESTS --

    [Fact]
    public async Task UpdateCategoryAsync_HappyPath_CallsUpdateAndSaveChanges()
    {
        // Arrange
        var existingCategory = new Category { Id = 1, Name = "Old Name" };

        // Act
        await _sut.UpdateCategoryAsync(existingCategory);

        // Assert
        _repositoryMock.Received(1).Update(existingCategory);
        await _repositoryMock.Received(1).SaveChangesAsync();
    }

    // --- DELETE TESTS ---

    [Fact]
    public async Task DeleteCategoryAsync_HappyPath_DeletesCategoryWhenItExists()
    {
        // Arrange
        var categoryToDelete = new Category { Id = 1, Name = "To Be Deleted" };
        _repositoryMock.GetByIdAsync(1).Returns(Task.FromResult<Category?>(categoryToDelete));

        // Act
        await _sut.DeleteCategoryAsync(1);

        // Assert
        _repositoryMock.Received(1).Delete(categoryToDelete);
        await _repositoryMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenObjectIsMissing_DoesNotCallDeleteOrSaveChanges()
    {
        // Arrange
        _repositoryMock.GetByIdAsync(999).Returns(Task.FromResult<Category?>(null));

        // Act
        await _sut.DeleteCategoryAsync(999);

        // Assert
        // Verifierar med NSubstitute att metoderna ALDRIG (DidNotReceive) anropades
        _repositoryMock.DidNotReceive().Delete(Arg.Any<Category>());
        await _repositoryMock.DidNotReceive().SaveChangesAsync();
    }
}