using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.Models;
using ToDoApp.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ToDoApp.Api.Services.Interfaces;
using ToDoApp.Api.Common;
using ToDoApp.Api.Extensions;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskItem>>> GetTasks([FromQuery] Guid userId)
        {
            var result = await _taskService.GetTasks(userId);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask([FromBody] TaskItem taskItem)
        {
            var result = await _taskService.PostTask(taskItem);

            if (result.Success is false)
            {
                return result.ToErrorActionResult(this);
            }

            return CreatedAtAction(nameof(GetTasks), new { result.Data!.UserId }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ModifyTask(int id, [FromBody] TaskItem taskItem)
        {
            var result = await _taskService.ModifyTask(id, taskItem);

            if (result.Success is false)
            {
                return result.ToErrorActionResult(this);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTask(id);

            if (result.Success is false)
            {
                return result.ToErrorActionResult(this);
            }

            return NoContent();
        }
    }
}
