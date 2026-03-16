using ToDoList.Domain.Entities;
using ToDoList.Repository.Contracts;
using ToDoList.Repository.Persistence;

namespace ToDoList.Repository.Implementations
{
    public class CategoryRepository(ApplicationDbContext context) : GenericRepository<Category>(context), ICategoryRepository;
}
