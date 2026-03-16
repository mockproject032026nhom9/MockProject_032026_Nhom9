namespace ToDoList.Service.DTOs
{
    public class TaskCreateDto
    {
        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; } 
    }
}
