using Microsoft.EntityFrameworkCore;
using ProjectFinally.Data;
using ProjectFinally.Repositories.Interfaces;
using TaskEntity = ProjectFinally.Models.Entities.Task;

namespace ProjectFinally.Repositories.Implementations;

public class TaskRepository : GenericRepository<TaskEntity>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetAllAsync()
    {
        return await _dbSet
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToEmployee)
                .ThenInclude(e => e.User)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByStatusAsync(string status)
    {
        return await _dbSet
            .Where(t => t.Status == status)
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToEmployee)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByPriorityAsync(string priority)
    {
        return await _dbSet
            .Where(t => t.Priority == priority)
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToEmployee)
            .OrderByDescending(t => t.DueDate)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksByEmployeeAsync(int userId)
    {
        // Buscar tareas asignadas a este usuario (a través de su registro de Employee)
        return await _dbSet
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToEmployee)
                .ThenInclude(e => e.User)
            .Where(t => t.AssignedToEmployee != null && t.AssignedToEmployee.UserId == userId)
            .OrderByDescending(t => t.DueDate)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetTasksCreatedByUserAsync(int userId)
    {
        return await _dbSet
            .Where(t => t.CreatedByUserId == userId)
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToEmployee)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskEntity>> GetOverdueTasksAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(t => t.DueDate.HasValue && t.DueDate.Value < now && t.Status != "Completed")
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToEmployee)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<TaskEntity?> GetTaskWithCommentsAsync(int taskId)
    {
        return await _dbSet
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToEmployee)
            .Include(t => t.Comments)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(t => t.TaskId == taskId);
    }
}
