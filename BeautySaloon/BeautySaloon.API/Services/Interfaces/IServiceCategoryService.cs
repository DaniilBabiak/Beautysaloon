using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Services.Interfaces;

public interface IServiceCategoryService
{
    Task<ServiceCategory> CreateCategoryAsync(ServiceCategory category);
    Task DeleteCategoryAsync(int id);
    Task<List<ServiceCategory>> GetAllCategoriesAsync();
    Task<ServiceCategory> GetCategoryByIdAsync(int id);
    Task<ServiceCategory> UpdateCategoryAsync(ServiceCategory category);
}
