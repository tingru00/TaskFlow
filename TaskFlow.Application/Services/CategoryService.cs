using System;
using System.Collections.Generic;
using System.Text;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

// Service som hanterar logiken för kategorier.
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // Hämtar alla kategorier.
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    // Hämtar en kategori baserat på id.
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    // Skapar en ny kategori.
    public async Task CreateCategoryAsync(Category category)
    {
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();
    }

    // Uppdaterar en kategori.
    public async Task UpdateCategoryAsync(Category category)
    {
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();
    }

    // Tar bort en kategori.
    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is not null)
        {
            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}