using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;

namespace ProjectFinally.Repositories.Implementations;

public class TaskCommentRepository : GenericRepository<TaskComment>, ITaskCommentRepository
{
    public TaskCommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TaskComment>> GetCommentsByTaskAsync(int taskId)
    {
        return await _dbSet
            .Where(c => c.TaskId == taskId)
            .Include(c => c.User)
            .Include(c => c.Task)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskComment>> GetCommentsByUserAsync(int userId)
    {
        return await _dbSet
            .Where(c => c.UserId == userId)
            .Include(c => c.User)
            .Include(c => c.Task)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}
