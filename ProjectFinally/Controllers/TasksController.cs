using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectFinally.Models.DTOs.Tasks;
using ProjectFinally.Services.Interfaces;
using System.Security.Claims;

namespace ProjectFinally.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks([FromQuery] int? userId = null)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int currentUserId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Admin: Ve TODAS las tareas o filtra por userId si se proporciona
            if (roleClaim == "Admin")
            {
                if (userId.HasValue)
                {
                    var filteredTasks = await _taskService.GetTasksCreatedByUserAsync(userId.Value);
                    return Ok(filteredTasks);
                }
                var allTasks = await _taskService.GetAllTasksAsync();
                return Ok(allTasks);
            }
            // Partner: Ve TODAS las tareas
            else if (roleClaim == "Partner")
            {
                var allTasks = await _taskService.GetAllTasksAsync();
                return Ok(allTasks);
            }
            // Employee: Ve solo SUS tareas (tareas asignadas a él)
            else if (roleClaim == "Employee")
            {
                var assignedTasks = await _taskService.GetTasksByEmployeeAsync(currentUserId);
                return Ok(assignedTasks);
            }
            // Viewer: Ve TODAS las tareas (solo lectura)
            else if (roleClaim == "Viewer")
            {
                var allTasks = await _taskService.GetAllTasksAsync();
                return Ok(allTasks);
            }
            // ContentManager: NO puede ver tareas
            else
            {
                return Forbid();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all tasks");
            return StatusCode(500, new { message = "An error occurred while retrieving tasks" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(int id)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound(new { message = $"Task with ID {id} not found" });

            return Ok(task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task {TaskId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the task" });
        }
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByStatus(string status)
    {
        try
        {
            var tasks = await _taskService.GetTasksByStatusAsync(status);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks with status {Status}", status);
            return StatusCode(500, new { message = "An error occurred while retrieving tasks" });
        }
    }

    [HttpGet("priority/{priority}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByPriority(string priority)
    {
        try
        {
            var tasks = await _taskService.GetTasksByPriorityAsync(priority);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks with priority {Priority}", priority);
            return StatusCode(500, new { message = "An error occurred while retrieving tasks" });
        }
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByEmployee(int employeeId)
    {
        try
        {
            var tasks = await _taskService.GetTasksByEmployeeAsync(employeeId);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks for employee {EmployeeId}", employeeId);
            return StatusCode(500, new { message = "An error occurred while retrieving tasks" });
        }
    }

    [HttpGet("my-tasks")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetMyTasks()
    {
        try
        {
            var userId = GetCurrentUserId();
            var tasks = await _taskService.GetTasksCreatedByUserAsync(userId);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks for current user");
            return StatusCode(500, new { message = "An error occurred while retrieving tasks" });
        }
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetOverdueTasks()
    {
        try
        {
            var tasks = await _taskService.GetOverdueTasksAsync();
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving overdue tasks");
            return StatusCode(500, new { message = "An error occurred while retrieving overdue tasks" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Partner,Employee")]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto createDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var task = await _taskService.CreateTaskAsync(createDto, userId);
            return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode(500, new { message = "An error occurred while creating the task" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Partner,Employee")]
    public async Task<ActionResult<TaskDto>> UpdateTask(int id, [FromBody] UpdateTaskDto updateDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Obtener la tarea existente para verificar propiedad
            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
                return NotFound(new { message = $"Task with ID {id} not found" });

            // Admin: puede editar cualquier tarea
            // Partner y Employee: solo pueden editar tareas creadas por ellos
            if (roleClaim == "Partner" || roleClaim == "Employee")
            {
                if (existingTask.CreatedByUserId != userId)
                {
                    return Forbid();
                }
            }

            var task = await _taskService.UpdateTaskAsync(id, updateDto);
            return Ok(task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task {TaskId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the task" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Partner,Employee")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Obtener la tarea existente para verificar propiedad
            var existingTask = await _taskService.GetTaskByIdAsync(id);
            if (existingTask == null)
                return NotFound(new { message = $"Task with ID {id} not found" });

            // Admin: puede eliminar cualquier tarea
            // Partner y Employee: solo pueden eliminar tareas creadas por ellos
            if (roleClaim == "Partner" || roleClaim == "Employee")
            {
                if (existingTask.CreatedByUserId != userId)
                {
                    return Forbid();
                }
            }

            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return NotFound(new { message = $"Task with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {TaskId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the task" });
        }
    }

    // Task Comments Endpoints
    [HttpGet("{taskId}/comments")]
    public async Task<ActionResult<IEnumerable<TaskCommentDto>>> GetTaskComments(int taskId)
    {
        try
        {
            var comments = await _taskService.GetCommentsByTaskAsync(taskId);
            return Ok(comments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving comments for task {TaskId}", taskId);
            return StatusCode(500, new { message = "An error occurred while retrieving comments" });
        }
    }

    [HttpPost("comments")]
    public async Task<ActionResult<TaskCommentDto>> AddComment([FromBody] CreateTaskCommentDto createDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var comment = await _taskService.AddCommentAsync(createDto, userId);
            return CreatedAtAction(nameof(GetTaskComments), new { taskId = comment.TaskId }, comment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding comment to task");
            return StatusCode(500, new { message = "An error occurred while adding the comment" });
        }
    }
}
