using ProjectFinally.Models.Entities;

namespace ProjectFinally.Repositories.Interfaces;

public interface ITaskCommentRepository : IGenericRepository<TaskComment>
{
    Task<IEnumerable<TaskComment>> GetCommentsByTaskAsync(int taskId);
    Task<IEnumerable<TaskComment>> GetCommentsByUserAsync(int userId);
}
