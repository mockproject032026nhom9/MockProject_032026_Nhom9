using ToDoList.Domain.Entities;
using ToDoList.Repository.Contracts;
using ToDoList.Repository.Implementations;
using ToDoList.Service.Common.Exception;
using ToDoList.Service.Contracts;
using ToDoList.Service.DTOs;

namespace ToDoList.Service.Implementations
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        public async Task<CategoryDto> AddCategoryAsync(CategoryCreateDto categoryDto)
        {

            var category = new Category(categoryDto.Name, categoryDto.Description);
            
            await categoryRepository.AddAsync(category);

            return new CategoryDto { Name = category.Name, Description = category.Description };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            if (id <= 0) throw new DomainValidationException(["id must be greater than zero"]);
            var isDeleted = await categoryRepository.DeleteAsync(id);
            return isDeleted;
        }

        public async Task<IReadOnlyCollection<CategoryDto>> GetAllCategoriesAsync()
        {
            var tasks = await categoryRepository.GetAllAsync();
            return tasks
                .Select(c => new CategoryDto()
                {
                    Name = c.Name,
                    Description = c.Description
                })
                .ToList();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            if (id <= 0) throw new DomainValidationException(["id must be greater than zero"]);
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null) return null;
            return new CategoryDto()
            {
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryDto> UpdateCategoryAsync(CategoryUpdateDto categoryDto, int id)
        {
            if (id <= 0) throw new DomainValidationException(["id must be greater than zero"]);

            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException($"Category with ID {id} does not exist.");

            category.UpdateCategory(categoryDto.Name, categoryDto.Description);

            await categoryRepository.UpdateAsync(category);
            return new CategoryDto { Name = category.Name, Description = category.Description };
        }
    }
}
