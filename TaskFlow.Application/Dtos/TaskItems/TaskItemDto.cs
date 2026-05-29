using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.DTOs.TaskItems;

// DTO för att visa en uppgift i API.
public class TaskItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDone { get; set; }
    public int CategoryId { get; set; }

    // För att visa kategorinamnet i API, även om det inte är nödvändigt.
    public string? CategoryName { get; set; }
}
