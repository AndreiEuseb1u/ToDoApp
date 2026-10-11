using ToDoApp.Common;
using ToDoApp.Models;

namespace ToDoApp.Services.Interfaces
{
    public interface ITaskApiService
    {
        Task<ApiResult<List<TaskItem>>> GetTasksAsync();
        Task<ApiResult<TaskItem>> PostTaskAsync(TaskItem taskItem);
        Task<ApiResult<TaskItem>> PutTaskAsync(int id, TaskItem taskItem);
        Task<ApiResult> DeleteTaskAsync(int id, uint rowVersion);
    }
}
