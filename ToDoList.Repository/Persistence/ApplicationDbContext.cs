using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Repository.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
