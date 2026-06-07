using System;
using System.Collections.Generic;
using System.Text;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

// Service som hanterar logiken för uppgifter.
public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;

    public TaskItemService(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    // Hämtar alla uppgifter.
    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return await _taskItemRepository.GetAllAsync();
    }

    // Hämtar en uppgift baserat på id.
    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await _taskItemRepository.GetByIdAsync(id);
    }

    // Skapar en ny uppgift.
    public async Task CreateTaskAsync(TaskItem taskItem)
    {
        if (taskItem is null)
        {
            throw new ArgumentNullException(nameof(taskItem), "Uppgiften kan inte vara null.");
        }

        await _taskItemRepository.AddAsync(taskItem);
        await _taskItemRepository.SaveChangesAsync();
    }

    // Uppdaterar en uppgift.
    public async Task UpdateTaskAsync(TaskItem taskItem)
    {
        _taskItemRepository.Update(taskItem);
        await _taskItemRepository.SaveChangesAsync();
    }

    // Tar bort en uppgift.
    public async Task DeleteTaskAsync(int id)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(id);

        if (taskItem is not null)
        {
            _taskItemRepository.Delete(taskItem);
            await _taskItemRepository.SaveChangesAsync();
        }
    }
}