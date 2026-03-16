using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Domain.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [StringLength(100)]
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; } = DateTime.Now;

        public IReadOnlyCollection<Task> Tasks => _tasks;

        private readonly List<Task> _tasks = [];

        private Category() { }

        public Category(string name, string description)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
        }

        public void CreateCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void UpdateCategory(string newName, string? newDescription)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Category name cannot be empty");

            Name = newName;
            if (newDescription != null) 
                Description = newDescription;
        }

        public Task AddTask(string title, string description, DateTime dueDate)
        {
            var task = new Task(title, description, dueDate);
            _tasks.Add(task);
            return task;
        }

        public void RemoveTask(Task task)
        {
            _tasks.Remove(task);
        }

        public Task UpdateTask(Task task, string newTitle, string newDescription, DateTime newDueDate)
        {
            if (!_tasks.Contains(task))
                throw new ArgumentException("Task does not belong to this category");
            task.UpdateTask(newTitle, newDescription, newDueDate);
            return task;
        }
    }
}
