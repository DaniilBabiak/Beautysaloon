using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ServiceModels;

namespace BeautySaloon.API.Services.Interfaces;

public interface IServiceService
{
    Task<ServiceDetailedModel> CreateServiceAsync(ServiceDetailedModel service);
    Task DeleteServiceAsync(int id);
    Task<List<ServiceCategory>> GetAllServiceCategoriesAsync();
    Task<List<ServiceModel>> GetAllServicesAsync();
    Task<ServiceDetailedModel> GetServiceByIdAsync(int id);
    Task<List<ServiceModel>> GetServicesByCategoryIdAsync(int id);
    Task<ServiceDetailedModel> UpdateServiceAsync(ServiceDetailedModel service);
}
