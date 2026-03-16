using ToDoList.Domain.Entities;
using ToDoList.Repository.Contracts;
using ToDoList.Repository.Implementations;
using ToDoList.Service.Common.Exception;
using ToDoList.Service.Contracts;
using ToDoList.Service.DTOs;

namespace ToDoList.Service.Implementations
{
    public class TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository) : ITaskService
    {
        public async Task<TaskDto> AddTaskAsync(TaskCreateDto taskDto)
        {

            var category = await categoryRepository.GetByIdAsync(taskDto.CategoryId);

            if (category == null)
            {
                throw new NotFoundException($"Category with ID {taskDto.CategoryId} does not exist.");
            }

            var task = category.AddTask(taskDto.Title, taskDto.Description, taskDto.DueDate);

            await taskRepository.AddAsync(task);

            return new TaskDto { Title = task.Title, Description = task.Description, DueDate = task.DueDate, CategoryId = task.CategoryId };
        }

        public async Task<TaskDto> UpdateTaskAsync(TaskUpdateDto taskDto, int id)
        {
            if (id <= 0) throw new DomainValidationException(["id must be greater than zero"]);
            var category = await categoryRepository.GetByIdAsync(taskDto.CategoryId);

            if (category == null)
            {
                throw new NotFoundException($"Category with ID {taskDto.CategoryId} does not exist.");
            }

            var existingTask = await taskRepository.GetByIdAsync(id);

            if (existingTask == null)
            {
                throw new NotFoundException($"Task with ID {id} does not exist.");
            }

            var task = category.UpdateTask(existingTask, taskDto.Title, taskDto.Description, taskDto.DueDate);

            await taskRepository.UpdateAsync(task);

            return new TaskDto { Title = task.Title, Description = task.Description, DueDate = task.DueDate, CategoryId = task.CategoryId };
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            if (id <= 0) throw new DomainValidationException(["id must be greater than zero"]);
            var task = await taskRepository.GetByIdAsync(id);

            if (task is null) return null;

            return new TaskDto
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                CategoryId = task.CategoryId
            };
        }

        public async Task<IReadOnlyCollection<List<TaskDto>>> GetAllTasksAsync()
        {
            var tasks = await taskRepository.GetAllAsync();
            return tasks.GroupBy(t => t.CategoryId)
                        .Select(g => g.Select(t => new TaskDto
                        {
                            Title = t.Title,
                            Description = t.Description,
                            DueDate = t.DueDate,
                            CategoryId = t.CategoryId
                        }).ToList())
                        .ToList();
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            if (id <= 0) throw new DomainValidationException(["id must be greater than zero"]);
            var isDeleted = await taskRepository.DeleteAsync(id);
            return isDeleted;
        }
    }
}
