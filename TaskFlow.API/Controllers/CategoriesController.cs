using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.Categories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Hämtar alla kategorier.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        var result = categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        });

        return Ok(result);
    }

    // Hämtar en kategori från id.
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        var result = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };

        return Ok(result);
    }

    // Skapar en ny kategori.
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };

        await _categoryService.CreateCategoryAsync(category);

        var result = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // Uppdaterar en kategori.
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        category.Name = dto.Name;

        await _categoryService.UpdateCategoryAsync(category);

        return NoContent();
    }

    // Tar bort en kategori.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        await _categoryService.DeleteCategoryAsync(id);

        return NoContent();
    }
}