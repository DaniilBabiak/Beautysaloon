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

    public async Task<CategoryModel> CreateCategoryAsync(CategoryModel categoryModel)
    {
        var newCategory = _mapper.Map<ServiceCategory>(categoryModel);

        newCategory.Services = new List<Service>();

        await _context.ServiceCategories.AddAsync(newCategory);
        await _context.SaveChangesAsync();

        if (categoryModel.ServiceIds is not null)
        {
            foreach (var serviceId in categoryModel.ServiceIds)
            {
                var service = await _context.Services.Where(s => s.Id == serviceId).FirstOrDefaultAsync();

                if (service is null)
                {
                    throw new ServiceNotFoundException($"Error while creating category. Service with id {serviceId} not found.");
                }

                newCategory.Services.Add(service);
                service.Category = newCategory;
                service.CategoryId = newCategory.Id;
            }
        }

        await _context.SaveChangesAsync();

        var result = _mapper.Map<CategoryModel>(newCategory);

        return categoryModel;
    }

    public async Task<CategoryModel> UpdateCategoryAsync(CategoryModel categoryModel)
    {
        var existingCategory = await _context.ServiceCategories
                                             .Include(c => c.Services)
                                             .FirstOrDefaultAsync(s => s.Id == categoryModel.Id);

        _mapper.Map(categoryModel, existingCategory);

        if (existingCategory is null)
        {
            throw new CategoryNotFoundException($"Category with id {categoryModel.Id} not found.");
        }

        existingCategory.Services = new List<Service>();

        if (categoryModel.ServiceIds is not null)
        {
            foreach (var serviceId in categoryModel.ServiceIds)
            {
                var service = await _context.Services.Where(s => s.Id == serviceId).FirstOrDefaultAsync();

                if (service is null)
                {
                    throw new ServiceNotFoundException($"Error while creating category. Service with id {serviceId} not found.");
                }

                existingCategory.Services.Add(service);
                service.Category = existingCategory;
                service.CategoryId = existingCategory.Id;
            }
        }

        await _context.SaveChangesAsync();

        var result = _mapper.Map<CategoryModel>(existingCategory);

        return result;
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
