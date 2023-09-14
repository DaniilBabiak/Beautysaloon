using BeautySaloon.API.Models.CategoryModels;

namespace BeautySaloon.API.Services.Interfaces;

public interface IServiceCategoryService
{
    Task<CategoryModel> CreateCategoryAsync(CategoryModel category);
    Task DeleteCategoryAsync(int id);
    Task<List<CategoryModel>> GetAllCategoriesAsync();
    Task<CategoryModel> GetCategoryByIdAsync(int id);
    Task<CategoryModel> UpdateCategoryAsync(CategoryModel category);
}
