using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.Common;
using ToDoApp.Api.Models;

namespace ToDoApp.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<ServiceResult<List<TaskItem>>> GetTasks(Guid userId);
        Task<ServiceResult<TaskItem>> PostTask(TaskItem taskItem);
        Task<ServiceResult<TaskItem>> ModifyTask(int id, TaskItem taskItem);
        Task<ServiceResult> DeleteTask(int id);
    }
}
