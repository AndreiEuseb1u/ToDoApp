using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.Models;
using ToDoApp.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ToDoApp.Api.Services.Interfaces;
using ToDoApp.Api.Common;
using ToDoApp.Api.Extensions;
using System.Security.Claims;

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
        public async Task<ActionResult<List<TaskItem>>> GetTasks()
        {
            if (this.TryGetUserId(out var userId) is false)
            {
                return Unauthorized("Could not determine the current user.");
            }

            var result = await _taskService.GetTasks(userId);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask([FromBody] TaskItem taskItem)
        {
            if (this.TryGetUserId(out var userId) is false)
            {
                return Unauthorized("Could not determine the current user.");
            }

            taskItem.UserId = userId;

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
            if (this.TryGetUserId(out var userId) is false)
            {
                return Unauthorized("Could not determine the current user.");
            }

            var result = await _taskService.ModifyTask(id, taskItem, userId);

            if (result.Success is false)
            {
                return result.ToErrorActionResult(this);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            if (this.TryGetUserId(out var userId) is false)
            {
                return Unauthorized("Could not determine the current user.");
            }

            var result = await _taskService.DeleteTask(id, userId);

            if (result.Success is false)
            {
                return result.ToErrorActionResult(this);
            }

            return NoContent();
        }

        [HttpGet("debug-claims")]
        public ActionResult DebugClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });

            return Ok(claims);
        }
    }
}
