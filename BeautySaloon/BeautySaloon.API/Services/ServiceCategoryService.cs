using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Models.CategoryModels;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class ServiceCategoryService : IServiceCategoryService
{
    private readonly BeautySaloonContext _context;
    private readonly IMapper _mapper;

    public ServiceCategoryService(BeautySaloonContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryModel>> GetAllCategoriesAsync()
    {
        var categories = await _context.ServiceCategories
                                   .AsNoTracking()
                                   .Include(c => c.Services)
                                       .ThenInclude(s => s.Reservations)
                                   .ToListAsync();

        var result = _mapper.Map<List<CategoryModel>>(categories);

        return result;
    }

    public async Task<CategoryModel> GetCategoryByIdAsync(int id)
    {
        var category = await _context.ServiceCategories
                                   .AsNoTracking()
                                   .Include(c => c.Services)
                                        .ThenInclude(s => s.Reservations)
                                   .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            throw new CategoryNotFoundException($"Category with id {id} not found!");
        }

        var result = _mapper.Map<CategoryModel>(category);

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
