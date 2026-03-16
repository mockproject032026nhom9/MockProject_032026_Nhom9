using ToDoList.Repository.Contracts;
using ToDoList.Repository.Persistence;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Repository.Implementations
{
    public class TaskRepository(ApplicationDbContext context) : GenericRepository<Task>(context), ITaskRepository
    {
    }
}
