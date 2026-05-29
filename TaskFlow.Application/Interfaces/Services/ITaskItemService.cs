using System;
using System.Collections.Generic;
using System.Text;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces.Services;

// Interface kopplad till uppgifter.
public interface ITaskItemService
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task CreateTaskAsync(TaskItem taskItem);
    Task UpdateTaskAsync(TaskItem taskItem);
    Task DeleteTaskAsync(int id);
}
