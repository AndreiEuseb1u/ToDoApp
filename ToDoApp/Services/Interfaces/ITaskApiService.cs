using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Common;
using ToDoApp.Models;

namespace ToDoApp.Services.Interfaces
{
    public interface ITaskApiService
    {
        Task<List<TaskItem>> GetTasksAsync();
        Task<TaskItem?> PostTaskAsync(TaskItem taskItem);
        Task<ApiResult<TaskItem>> PutTaskAsync(int id, TaskItem taskItem);
        Task<ApiResult> DeleteTaskAsync(int id, uint rowVersion);
    }
}
