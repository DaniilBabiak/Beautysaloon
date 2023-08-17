using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Services.Interfaces;

public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Customer> GetCustomerAsync(string id);
    Task<Customer> UpdateCustomerAsync(Customer customer);
}
