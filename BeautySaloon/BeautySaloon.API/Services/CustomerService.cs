using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Helpers;
using BeautySaloon.API.Services.Interfaces;
using BeautySaloon.API.Validators;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class CustomerService : ICustomerService
{
    private readonly BeautySaloonContext _context;
    private readonly CustomerValidator _customerValidator;
    public CustomerService(BeautySaloonContext context)
    {
        _context = context;
        _customerValidator = new CustomerValidator();
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        var existingCustomer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id.Equals(customer.Id));

        if (existingCustomer is not null)
        {
            throw new CustomerAlreadyExistsException($"Customer with id {customer.Id} already exists.");
        }

        var result = await _customerValidator.ValidateAsync(customer);

        if (!result.IsValid)
        {
            throw new CustomerNotValidException(result);
        }

        if (!string.IsNullOrEmpty(customer.PhoneNumber))
        {
            customer.PhoneNumber = PhoneNumberHelper.ConvertToE164PhoneNumber(customer.PhoneNumber);
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

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        var existingCustomer = await _context.Customers
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(c => c.Id.Equals(customer.Id));

        if (customer is null)
        {
            throw new CustomerNotFoundException($"Customer with id {customer.Id} not found.");
        }

        var result = await _customerValidator.ValidateAsync(customer);
        if (!result.IsValid)
        {
            throw new CustomerNotValidException(result);
        }

        customer.PhoneNumber = PhoneNumberHelper.ConvertToE164PhoneNumber(customer.PhoneNumber);

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();

        return customer;
    }



}
