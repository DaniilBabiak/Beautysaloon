using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class ServiceService : IServiceService
{
    private readonly BeautySaloonContext _context;

    public ServiceService(BeautySaloonContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceCategory>> GetAllServiceCategoriesAsync()
    {
        var result = await _context.ServiceCategories
                                   .AsNoTracking()
                                   .Include(c => c.Services)
                                       .ThenInclude(s => s.Reservations)
                                   .Include(c => c.Services)
                                       .ThenInclude(s => s.Category)
                                   .ToListAsync();

        return result;
    }

    public async Task<List<Service>> GetAllServicesAsync()
    {
        var result = await _context.Services.Include(c => c.Category).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<List<Service>> GetServicesByCategoryIdAsync(int id)
    {
        var category = await _context.ServiceCategories
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            throw new CategoryNotFoundException($"Category with id {id} not found!");
        }

        var result = await _context.Services
                                   .AsNoTracking()
                                   .Include(s => s.Masters)
                                   .Include(s => s.Category)
                                   .Include(s => s.Reservations)
                                   .Where(s => s.CategoryId == id)
                                   .ToListAsync();


        return result;
    }

    public async Task<Service> CreateServiceAsync(Service service)
    {
        if (service.CategoryId.HasValue)
        {
            var category = await _context.ServiceCategories
                             .Include(c => c.Services)
                             .FirstOrDefaultAsync(c => c.Id == service.CategoryId);

            if (category is null)
            {
                throw new CategoryNotFoundException($"Category with id {service.CategoryId} not found!");
            }
            category.Services ??= new List<Service>();
            category.Services.Add(service);

            await _context.SaveChangesAsync();

            return service;
        }

        await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();

        return service;
    }

    public async Task<Service> UpdateServiceAsync(Service service)
    {
        var existingService = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == service.Id);

        if (existingService is null)
        {
            throw new ServiceNotFoundException($"Service with id {service.Id} not found.");
        }

        _context.Services.Update(service);
        await _context.SaveChangesAsync();

        return service;
    }

    public async Task DeleteServiceAsync(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

        if (service is null)
        {
            throw new ServiceNotFoundException($"Service with id {id} not found.");
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
    }
}
