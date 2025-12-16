using TaskEntity = ProjectFinally.Models.Entities.Task;

namespace ProjectFinally.Repositories.Interfaces;

public interface ITaskRepository : IGenericRepository<TaskEntity>
{
    Task<IEnumerable<TaskEntity>> GetTasksByStatusAsync(string status);
    Task<IEnumerable<TaskEntity>> GetTasksByPriorityAsync(string priority);
    Task<IEnumerable<TaskEntity>> GetTasksByEmployeeAsync(int employeeId);
    Task<IEnumerable<TaskEntity>> GetTasksCreatedByUserAsync(int userId);
    Task<IEnumerable<TaskEntity>> GetOverdueTasksAsync();
    Task<TaskEntity?> GetTaskWithCommentsAsync(int taskId);
}
