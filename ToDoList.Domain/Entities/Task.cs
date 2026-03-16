using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Domain.Entities
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [StringLength(200)]
        public string Title { get; private set; }

        public string Description { get; private set; }

        public DateTime DueDate { get; private set; }
        public DateTime CreatedAt { get; } = DateTime.Now;
        [ForeignKey("CategoryId")]
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }

        private Task() {}

        public Task(string title, string description, DateTime dueDate)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            CreatedAt = DateTime.Now;
        }

        public void UpdateTask(string newTitle, string newDescription, DateTime newDueDate)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Task title cannot be empty");

            Title = newTitle;
            Description = newDescription;
            DueDate = newDueDate;
        }
    }
}
