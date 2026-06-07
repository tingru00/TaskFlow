using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.DTOs.TaskItems;

// DTO när en uppgift ska uppdateras.
public class UpdateTaskItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDone { get; set; }
    public int CategoryId { get; set; }
}
