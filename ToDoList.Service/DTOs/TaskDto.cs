using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Service.DTOs
{
    public class TaskDto
    {
        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; }
    }
}
