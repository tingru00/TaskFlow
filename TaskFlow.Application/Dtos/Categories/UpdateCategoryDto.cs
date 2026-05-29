using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.DTOs.Categories;

// DTO när en kategori ska uppdateras.
public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
}
