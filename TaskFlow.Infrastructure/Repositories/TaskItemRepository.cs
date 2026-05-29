using System;
using System.Collections.Generic;
using System.Text;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Repositories;

public class TaskItemRepository : GenericRepository<TaskItem>, ITaskItemRepository
{
    public TaskItemRepository(AppDbContext context) : base(context)
    {
    }
}