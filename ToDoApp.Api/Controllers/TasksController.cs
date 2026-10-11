using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using ToDoApp.Api.Services.Interfaces;
using ToDoApp.Api.Extensions;
using ToDoApp.Api.Dtos;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskItem>>> GetTasks()
        {
            if (this.TryGetUserId(out var userId) is false)
            {
                _logger.LogError("Could not determine the user id from the authenticated token in GetTasks. Request rejected.");
                return Unauthorized("Could not determine the current user.");
            }

            var result = await _taskService.GetTasks(userId);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTask([FromBody] CreateTaskRequest request)
        {
            if (this.TryGetUserId(out var userId) is false)
            {
                return Unauthorized("Could not determine the current user.");
            }

            var result = await _taskService.PostTask(request, userId);

            if (result.Success is false)
            {
                _logger.LogError("Could not determine the user id from the authenticated token in PostTask. Request rejected.");
                return result.ToErrorActionResult(this);
            }

            return CreatedAtAction(nameof(GetTasks), new { result.Data!.UserId }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ModifyTask(int id, [FromBody] ModifyTaskRequest request)
        {
            if (this.TryGetUserId(out var userId) is false)
            {
                _logger.LogError("Could not determine the user id from the authenticated token in ModifyTask for task {TaskId}. " +
                    "Request rejected.", id);
                return Unauthorized("Could not determine the current user.");
            }

            var result = await _taskService.ModifyTask(id, request, userId);

            if (result.Success is false)
            {
                return result.ToErrorActionResult(this);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id, uint rowVersion)
        {
            if (this.TryGetUserId(out var userId) is false)
            {
                _logger.LogError("Could not determine the user id from the authenticated token in DeleteTask for task {TaskId}. " +
                    "Request rejected.", id);
                return Unauthorized("Could not determine the current user.");
            }

            var result = await _taskService.DeleteTask(id, userId, rowVersion);

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
