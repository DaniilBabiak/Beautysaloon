using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Services.Interfaces;

public interface IServiceService
{
    Task<Service> CreateServiceAsync(Service service);
    Task DeleteServiceAsync(int id);
    Task<List<ServiceCategory>> GetAllServiceCategoriesAsync();
    Task<List<Service>> GetAllServicesAsync();
    Task<Service> GetServiceByIdAsync(int id);
    Task<List<Service>> GetServicesByCategoryIdAsync(int id);
    Task<Service> UpdateServiceAsync(Service service);
}
