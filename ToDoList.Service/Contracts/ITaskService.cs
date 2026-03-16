using ToDoList.Service.DTOs;

namespace ToDoList.Service.Contracts
{
    public interface ITaskService
    {
        public Task<TaskDto> AddTaskAsync(TaskCreateDto task);
        public Task<TaskDto> UpdateTaskAsync(TaskUpdateDto task, int id);
        public Task<TaskDto?> GetTaskByIdAsync(int id);
        public Task<IReadOnlyCollection<List<TaskDto>>> GetAllTasksAsync();
        public Task<bool> DeleteTaskAsync(int id);
    }
}
