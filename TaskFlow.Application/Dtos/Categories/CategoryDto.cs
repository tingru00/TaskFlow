using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.DTOs.Categories;

// DTO för att visa en kategori i API.
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
