using ToDoApp.Api.Data;
using ToDoApp.Api.Models;
using ToDoApp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Common;

namespace ToDoApp.Api.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _appDbContext;

        public TaskService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ServiceResult<List<TaskItem>>> GetTasks(Guid userId)
        {
            var taskList =  await _appDbContext.Tasks.Where(t => t.UserId == userId).ToListAsync();

            return ServiceResult<List<TaskItem>>.Ok(taskList);
        }

        public async Task<ServiceResult<TaskItem>> PostTask(TaskItem taskItem)
        {

            if (string.IsNullOrWhiteSpace(taskItem.TaskDescription))
            {
                return ServiceResult<TaskItem>.Fail("Task description cannot be empty", ServiceErrorType.ValidationFailed);
            }

            await _appDbContext.Tasks.AddAsync(taskItem);

            await _appDbContext.SaveChangesAsync();

            return ServiceResult<TaskItem>.Ok(taskItem);
        }

        public async Task<ServiceResult<TaskItem>> ModifyTask(int id, TaskItem taskItem)
        {
            var task = await _appDbContext.Tasks.FindAsync(id);

            if (task is null)
            {
                return ServiceResult<TaskItem>.Fail("Task not found", ServiceErrorType.NotFound);
            }

            if (string.IsNullOrWhiteSpace(taskItem.TaskDescription))
            {
                return ServiceResult<TaskItem>.Fail("Task description cannot be empty", ServiceErrorType.ValidationFailed);
            }

            task.TaskDescription = taskItem.TaskDescription;

            task.IsCompleted = taskItem.IsCompleted;

            await _appDbContext.SaveChangesAsync();

            return ServiceResult<TaskItem>.Ok(task);
        }

        public async Task<ServiceResult> DeleteTask(int id)
        {
            var task = await _appDbContext.Tasks.FindAsync(id);

            if (task is null)
            {
                return ServiceResult.Fail("Task not found", ServiceErrorType.NotFound);
            }

            _appDbContext.Tasks.Remove(task);

            await _appDbContext.SaveChangesAsync();

            return ServiceResult.Ok();
        }
    }
}
