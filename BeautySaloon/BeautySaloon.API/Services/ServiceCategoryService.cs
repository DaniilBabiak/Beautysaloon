using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class ServiceCategoryService : IServiceCategoryService
{
    private readonly BeautySaloonContext _context;

    public ServiceCategoryService(BeautySaloonContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceCategory>> GetAllCategoriesAsync()
    {
        var result = await _context.ServiceCategories
                                   .AsNoTracking()
                                   .Include(c => c.Services)
                                       .ThenInclude(s => s.Reservations)
                                   .ToListAsync();
        return result;
    }

    public async Task<ServiceCategory> GetCategoryByIdAsync(int id)
    {
        var result = await _context.ServiceCategories
                                   .AsNoTracking()
                                   .Include(c => c.Services)
                                        .ThenInclude(s => s.Reservations)
                                   .FirstOrDefaultAsync(c => c.Id == id);

        if (result is null)
        {
            throw new CategoryNotFoundException($"Category with id {id} not found!");
        }

        return result;
    }

    public async Task<ServiceCategory> CreateCategoryAsync(ServiceCategory category)
    {
        await _context.ServiceCategories.AddAsync(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<ServiceCategory> UpdateCategoryAsync(ServiceCategory category)
    {
        var existingCategory = await _context.ServiceCategories
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(s => s.Id == category.Id);

        if (existingCategory is null)
        {
            throw new CategoryNotFoundException($"Category with id {category.Id} not found.");
        }

        _context.ServiceCategories.Update(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.ServiceCategories
                                     .Include(c => c.Services)
                                     .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            throw new CategoryNotFoundException($"Category with id {id} not found!");
        }

        if (category.Services.Any())
        {
            foreach (var service in category.Services)
            {
                service.CategoryId = null;
            }
        }

        _context.ServiceCategories.Remove(category);

        await _context.SaveChangesAsync();
    }
}
