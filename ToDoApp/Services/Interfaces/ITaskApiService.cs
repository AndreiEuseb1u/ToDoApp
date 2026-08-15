using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Models;

namespace ToDoApp.Services.Interfaces
{
    public interface ITaskApiService
    {
        Task<List<TaskItem>> GetTasksAsync(Guid id);
        Task<TaskItem?> PostTaskAsync(TaskItem taskItem);
        Task PutTaskAsync(int id, TaskItem taskItem);
        Task DeleteTaskAsync(int id);
    }
}
