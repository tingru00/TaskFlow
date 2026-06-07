using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDone { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
