using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.DTOs.Categories;

// DTO när en ny kategori ska skapas.
public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
}