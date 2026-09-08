using TaskManager.Models;

namespace TaskManager.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetTasksAsync(int userId);

    Task<TaskItem?> GetTaskByIdAsync(int taskId, int userId);

    Task<TaskItem> CreateTaskAsync(
        int userId,
        string title,
        string? description
    );

    Task<TaskItem?> UpdateTaskAsync(
        int taskId,
        int userId,
        string title,
        string? description,
        bool isCompleted
    );

    Task<bool> DeleteTaskAsync(int taskId, int userId);
}