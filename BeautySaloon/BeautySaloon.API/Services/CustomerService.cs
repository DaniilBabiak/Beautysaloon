using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class CustomerService : ICustomerService
{
    private readonly BeautySaloonContext _context;

    public CustomerService(BeautySaloonContext context)
    {
        _context = context;
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        var existingCustomer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id.Equals(customer.Id));

        if (existingCustomer is not null)
        {
            throw new CustomerAlreadyExistsException($"Customer with id {customer.Id} already exists.");
        }

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<Customer> GetCustomerAsync(string id)
    {
        var customer = await _context.Customers
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.Id.Equals(id));

        if (customer is null)
        {
            throw new CustomerNotFoundException($"Customer with id {id} not found.");
        }

        return customer;
    }
}
