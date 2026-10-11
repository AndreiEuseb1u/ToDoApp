using ToDoApp.Common;
using ToDoApp.Dtos;
using ToDoApp.Models;

namespace ToDoApp.Services.Interfaces
{
    public interface ITaskApiService
    {
        Task<ApiResult<List<TaskItem>>> GetTasksAsync();
        Task<ApiResult<TaskItem>> PostTaskAsync(CreateTaskRequest request);
        Task<ApiResult<TaskItem>> PutTaskAsync(int id, ModifyTaskRequest request);
        Task<ApiResult> DeleteTaskAsync(int id, uint rowVersion);
    }
}
