using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetTasksAsync(int userId)
    {
        return await _context.TaskItems
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(
        int taskId,
        int userId)
    {
        return await _context.TaskItems
            .AsNoTracking()
            .FirstOrDefaultAsync(t =>
                t.Id == taskId &&
                t.UserId == userId
            );
    }

    public async Task<TaskItem> CreateTaskAsync(
        int userId,
        string title,
        string? description)
    {
        var task = new TaskItem
        {
            Title = title,
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        _context.TaskItems.Add(task);

        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<TaskItem?> UpdateTaskAsync(
        int taskId,
        int userId,
        string title,
        string? description,
        bool isCompleted)
    {
        var task = await _context.TaskItems
            .FirstOrDefaultAsync(t =>
                t.Id == taskId &&
                t.UserId == userId
            );

        if (task == null)
        {
            return null;
        }

        task.Title = title;
        task.Description = description;
        task.IsCompleted = isCompleted;

        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<bool> DeleteTaskAsync(
        int taskId,
        int userId)
    {
        var task = await _context.TaskItems
            .FirstOrDefaultAsync(t =>
                t.Id == taskId &&
                t.UserId == userId
            );

        if (task == null)
        {
            return false;
        }

        _context.TaskItems.Remove(task);

        await _context.SaveChangesAsync();

        return true;
    }
}