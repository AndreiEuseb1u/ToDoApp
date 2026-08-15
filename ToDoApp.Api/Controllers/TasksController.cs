using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.Models;
using ToDoApp.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public TasksController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskItem>>> GetTasks([FromQuery] Guid userId)
        {
            var tasks = await _appDbContext
                .Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask([FromBody] TaskItem taskItem)
        {
            if (string
                .IsNullOrWhiteSpace
                (taskItem.TaskDescription))
            {
                return BadRequest();
            }

            await _appDbContext
                .Tasks
                .AddAsync(taskItem);

            await _appDbContext
                .SaveChangesAsync();

            return Ok(taskItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ModifyTask(int id, [FromBody] TaskItem taskItem)
        {
            var task = await _appDbContext
                .Tasks
                .FindAsync(id);

            if (task is null)
            {
                return NotFound();
            }

            if (string
                .IsNullOrWhiteSpace
                (taskItem.TaskDescription))
            {
                return BadRequest();
            }

            task.TaskDescription = taskItem.TaskDescription;

            task.IsCompleted = taskItem.IsCompleted;

            await _appDbContext
                .SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var task = await _appDbContext
                .Tasks
                .FindAsync(id);

            if (task is null)
            {
                return NotFound();
            }

            _appDbContext
                .Tasks
                .Remove(task);

            await _appDbContext
                .SaveChangesAsync();

            return NoContent();
        }
    }
}
