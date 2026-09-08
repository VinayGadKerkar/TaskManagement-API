using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskItem>>> GetTasks()
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var tasks = await _taskService.GetTasksAsync(userId);

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask(
        CreateTaskDto dto)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var task = await _taskService.CreateTaskAsync(
            userId,
            dto.Title,
            dto.Description
        );

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = task.Id },
            task
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTaskById(int id)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var task = await _taskService.GetTaskByIdAsync(
            id,
            userId
        );

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> UpdateTask(
        int id,
        UpdateTaskDto dto)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var task = await _taskService.UpdateTaskAsync(
            id,
            userId,
            dto.Title,
            dto.Description,
            dto.IsCompleted
        );

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var deleted = await _taskService.DeleteTaskAsync(
            id,
            userId
        );

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}