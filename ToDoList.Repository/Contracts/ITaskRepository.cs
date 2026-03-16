using ToDoList.Repository.Contracts.Common;
using Task = ToDoList.Domain.Entities.Task;

namespace ToDoList.Repository.Contracts
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
    }
}
