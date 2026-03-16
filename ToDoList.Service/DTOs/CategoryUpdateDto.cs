namespace ToDoList.Service.DTOs
{
    public class CategoryUpdateDto
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
