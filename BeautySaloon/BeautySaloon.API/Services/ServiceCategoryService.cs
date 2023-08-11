using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
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
        var result = await _context.ServiceCategories.Include(c => c.Services).ToListAsync();
        return result;
    }

    public async Task<ServiceCategory> GetCategoryByIdAsync(int id)
    {
        var result = await _context.ServiceCategories.FirstOrDefaultAsync(c => c.Id == id);

        return result;
    }

    public async Task<ServiceCategory> GetServiceCategoryAsync(int id)
    {
        var result = await _context.ServiceCategories.Include(c => c.Services)
                                                     .SingleOrDefaultAsync(c => c.Id == id);

        return result;
    }

    public async Task<ServiceCategory> CreateCategoryAsync(ServiceCategory category)
    {
        await _context.ServiceCategories.AddAsync(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.ServiceCategories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            throw new ArgumentException($"Category with id {id} not found!");
        }

        _context.ServiceCategories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
