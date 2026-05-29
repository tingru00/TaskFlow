using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.DTOs.TaskItems;

// DTO när en ny uppgift ska skapas.
public class CreateTaskItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDone { get; set; }
    public int CategoryId { get; set; }
}