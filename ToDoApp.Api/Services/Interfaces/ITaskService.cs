using ToDoApp.Api.Common;
using ToDoApp.Api.Dtos;
using ToDoApp.Api.Models;

namespace ToDoApp.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<ServiceResult<List<TaskItem>>> GetTasks(Guid userId);
        Task<ServiceResult<TaskItem>> PostTask(CreateTaskRequest request, Guid userId);
        Task<ServiceResult<TaskItem>> ModifyTask(int id, ModifyTaskRequest request, Guid userId);
        Task<ServiceResult> DeleteTask(int id, Guid userId, uint rowVersion);
    }
}
