using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs.TaskItems;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemsController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    // Hämtar alla uppgifter.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAll()
    {
        var tasks = await _taskItemService.GetAllTasksAsync();

        var result = tasks.Select(task => new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsDone = task.IsDone,
            CategoryId = task.CategoryId,
            CategoryName = task.Category?.Name
        });

        return Ok(result);
    }

    // Hämtar en uppgift baserat på id.
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetById(int id)
    {
        var task = await _taskItemService.GetTaskByIdAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        var result = new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsDone = task.IsDone,
            CategoryId = task.CategoryId,
            CategoryName = task.Category?.Name
        };

        return Ok(result);
    }

    // Skapar en ny uppgift.
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> Create(CreateTaskItemDto dto)
    {
        var taskItem = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsDone = dto.IsDone,
            CategoryId = dto.CategoryId
        };

        await _taskItemService.CreateTaskAsync(taskItem);

        var result = new TaskItemDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            IsDone = taskItem.IsDone,
            CategoryId = taskItem.CategoryId,
            CategoryName = null
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // Uppdaterar en uppgift.
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTaskItemDto dto)
    {
        var taskItem = await _taskItemService.GetTaskByIdAsync(id);

        if (taskItem is null)
        {
            return NotFound();
        }

        taskItem.Title = dto.Title;
        taskItem.Description = dto.Description;
        taskItem.IsDone = dto.IsDone;
        taskItem.CategoryId = dto.CategoryId;

        await _taskItemService.UpdateTaskAsync(taskItem);

        return NoContent();
    }

    // Tar bort en uppgift.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var taskItem = await _taskItemService.GetTaskByIdAsync(id);

        if (taskItem is null)
        {
            return NotFound();
        }

        await _taskItemService.DeleteTaskAsync(id);

        return NoContent();
    }
}