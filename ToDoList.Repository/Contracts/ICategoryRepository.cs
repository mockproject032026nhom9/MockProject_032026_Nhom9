using ToDoList.Domain.Entities;
using ToDoList.Repository.Contracts.Common;

namespace ToDoList.Repository.Contracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
    }
}
