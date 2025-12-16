using AutoMapper;
using ProjectFinally.Models.DTOs.Tasks;
using ProjectFinally.Models.Entities;
using ProjectFinally.Repositories.Interfaces;
using ProjectFinally.Services.Interfaces;
using TaskEntity = ProjectFinally.Models.Entities.Task;

namespace ProjectFinally.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskCommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public TaskService(
        ITaskRepository taskRepository,
        ITaskCommentRepository commentRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async System.Threading.Tasks.Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetTaskWithCommentsAsync(id);
        return task == null ? null : _mapper.Map<TaskDto>(task);
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetTasksByStatusAsync(string status)
    {
        var tasks = await _taskRepository.GetTasksByStatusAsync(status);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetTasksByPriorityAsync(string priority)
    {
        var tasks = await _taskRepository.GetTasksByPriorityAsync(priority);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetTasksByEmployeeAsync(int employeeId)
    {
        var tasks = await _taskRepository.GetTasksByEmployeeAsync(employeeId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetTasksCreatedByUserAsync(int userId)
    {
        var tasks = await _taskRepository.GetTasksCreatedByUserAsync(userId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> GetOverdueTasksAsync()
    {
        var tasks = await _taskRepository.GetOverdueTasksAsync();
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async System.Threading.Tasks.Task<TaskDto> CreateTaskAsync(CreateTaskDto createDto, int createdByUserId)
    {
        var task = _mapper.Map<TaskEntity>(createDto);
        task.CreatedByUserId = createdByUserId;
        task.CreatedAt = DateTime.UtcNow;
        task.Status = "Pending";

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        var createdTask = await _taskRepository.GetTaskWithCommentsAsync(task.TaskId);
        return _mapper.Map<TaskDto>(createdTask!);
    }

    public async System.Threading.Tasks.Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateDto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return null;

        var oldStatus = task.Status;
        _mapper.Map(updateDto, task);
        task.UpdatedAt = DateTime.UtcNow;

        // Set CompletedDate when status changes to Completed
        if (updateDto.Status == "Completed" && oldStatus != "Completed")
        {
            task.CompletedDate = DateTime.UtcNow;
        }
        else if (updateDto.Status != "Completed")
        {
            task.CompletedDate = null;
        }

        _taskRepository.Update(task);
        await _taskRepository.SaveChangesAsync();

        var updatedTask = await _taskRepository.GetTaskWithCommentsAsync(id);
        return _mapper.Map<TaskDto>(updatedTask!);
    }

    public async System.Threading.Tasks.Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return false;

        _taskRepository.Delete(task);
        return await _taskRepository.SaveChangesAsync();
    }

    // Task Comments
    public async System.Threading.Tasks.Task<IEnumerable<TaskCommentDto>> GetCommentsByTaskAsync(int taskId)
    {
        var comments = await _commentRepository.GetCommentsByTaskAsync(taskId);
        return _mapper.Map<IEnumerable<TaskCommentDto>>(comments);
    }

    public async System.Threading.Tasks.Task<TaskCommentDto> AddCommentAsync(CreateTaskCommentDto createDto, int userId)
    {
        var comment = _mapper.Map<TaskComment>(createDto);
        comment.UserId = userId;
        comment.CreatedAt = DateTime.UtcNow;

        await _commentRepository.AddAsync(comment);
        await _commentRepository.SaveChangesAsync();

        var createdComment = await _commentRepository.GetByIdAsync(comment.CommentId);
        return _mapper.Map<TaskCommentDto>(createdComment!);
    }
}
