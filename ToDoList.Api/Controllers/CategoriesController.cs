using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Service.Common;
using ToDoList.Service.Contracts;
using ToDoList.Service.DTOs;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            return Ok(ApiResponse<IReadOnlyCollection<CategoryDto>>.Ok(categories));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound(ApiResponse<object>.Fail($"Category with ID {id} not found."));
            return Ok(ApiResponse<CategoryDto>.Ok(category));
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryCreateDto categoryDto)
        {
            var createdCategory = await categoryService.AddCategoryAsync(categoryDto);
            return Ok(ApiResponse<CategoryDto>.Ok(createdCategory));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryDto)
        {
            var updatedCategory = await categoryService.UpdateCategoryAsync(categoryDto, id);
            return Ok(ApiResponse<CategoryDto>.Ok(updatedCategory));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var isDeleted = await categoryService.DeleteCategoryAsync(id);
            if (!isDeleted) return NotFound(ApiResponse<object>.Fail($"Category with ID {id} not found."));
            return Ok(ApiResponse<object>.Ok(null, "Category deleted successfully."));
        }
    }
}
