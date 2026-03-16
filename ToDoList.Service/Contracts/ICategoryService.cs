using ToDoList.Domain.Entities;
using ToDoList.Service.DTOs;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Service.Contracts
{
    public interface ICategoryService
    {
        public Task<CategoryDto> AddCategoryAsync(CategoryCreateDto categoryDto);
        public Task<CategoryDto> UpdateCategoryAsync(CategoryUpdateDto categoryDto, int id);
        public Task<CategoryDto?> GetCategoryByIdAsync(int id);
        public Task<IReadOnlyCollection<CategoryDto>> GetAllCategoriesAsync();
        public Task<bool> DeleteCategoryAsync(int id);
    }
}
