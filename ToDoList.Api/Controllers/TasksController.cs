using Microsoft.AspNetCore.Mvc;
using ToDoList.Service.Common;
using ToDoList.Service.Contracts;
using ToDoList.Service.DTOs;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(ITaskService taskService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await taskService.GetAllTasksAsync();
            return Ok(ApiResponse<IReadOnlyCollection<List<TaskDto>>>.Ok(tasks));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound(ApiResponse<TaskDto>.Fail($"Task with ID {id} not found.", statusCode: 404));
            return Ok(ApiResponse<TaskDto>.Ok(task));
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskCreateDto taskDto)
        {
            var taskAdded = await taskService.AddTaskAsync(taskDto);
            return Ok(ApiResponse<TaskDto>.Ok(taskAdded, "Task added successfully."));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto taskDto)
        {
            var taskUpdated = await taskService.UpdateTaskAsync(taskDto, id);
            return Ok(ApiResponse<TaskDto>.Ok(taskUpdated, "Task updated successfully."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var isDeleted = await taskService.DeleteTaskAsync(id);
            if (!isDeleted)
                return NotFound(ApiResponse<TaskDto>.Fail($"Task with ID {id} not found.", statusCode: 404));
            return Ok(ApiResponse<TaskDto>.Ok(null, "Task deleted successfully."));
        }
    }
}
