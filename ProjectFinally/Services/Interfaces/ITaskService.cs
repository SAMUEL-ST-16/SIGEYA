using ProjectFinally.Models.DTOs.Tasks;

namespace ProjectFinally.Services.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskDto>> GetTasksByStatusAsync(string status);
    Task<IEnumerable<TaskDto>> GetTasksByPriorityAsync(string priority);
    Task<IEnumerable<TaskDto>> GetTasksByEmployeeAsync(int employeeId);
    Task<IEnumerable<TaskDto>> GetTasksCreatedByUserAsync(int userId);
    Task<IEnumerable<TaskDto>> GetOverdueTasksAsync();
    Task<TaskDto> CreateTaskAsync(CreateTaskDto createDto, int createdByUserId);
    Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateDto);
    Task<bool> DeleteTaskAsync(int id);

    // Task Comments
    Task<IEnumerable<TaskCommentDto>> GetCommentsByTaskAsync(int taskId);
    Task<TaskCommentDto> AddCommentAsync(CreateTaskCommentDto createDto, int userId);
}
