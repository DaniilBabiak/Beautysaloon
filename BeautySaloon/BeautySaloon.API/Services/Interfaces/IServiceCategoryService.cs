using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.CategoryModels;

namespace BeautySaloon.API.Services.Interfaces;

public interface IServiceCategoryService
{
    Task<ServiceCategory> CreateCategoryAsync(ServiceCategory category);
    Task DeleteCategoryAsync(int id);
    Task<List<CategoryModel>> GetAllCategoriesAsync();
    Task<CategoryModel> GetCategoryByIdAsync(int id);
    Task<ServiceCategory> UpdateCategoryAsync(ServiceCategory category);
}
