using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Models.ServiceModels;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class ServiceService : IServiceService
{
    private readonly BeautySaloonContext _context;
    private readonly IMapper _mapper;

    public ServiceService(BeautySaloonContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceDetailedModel> GetServiceByIdAsync(int id)
    {
        var service = await _context.Services
                             .Include(s => s.Category)
                             .Include(s => s.Masters)
                             .Include(s => s.Reservations)
                             .FirstOrDefaultAsync(s => s.Id == id);

        if (service is null)
        {
            throw new ServiceNotFoundException($"Service with id {id} not found.");
        }

        var result = _mapper.Map<ServiceDetailedModel>(service);
        return result;
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

    public async Task<List<ServiceModel>> GetAllServicesAsync()
    {
        var services = await _context.Services.Include(c => c.Category).AsNoTracking().ToListAsync();

        var result = _mapper.Map<List<ServiceModel>>(services);
        return result;
    }

    public async Task<List<ServiceModel>> GetServicesByCategoryIdAsync(int id)
    {
        var category = await _context.ServiceCategories
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            throw new CategoryNotFoundException($"Category with id {id} not found!");
        }

        var services = await _context.Services
                                   .AsNoTracking()
                                   .Include(s => s.Masters)
                                   .Include(s => s.Category)
                                   .Include(s => s.Reservations)
                                   .Where(s => s.CategoryId == id)
                                   .ToListAsync();

        var result = _mapper.Map<List<ServiceModel>>(services);
        return result;
    }

    public async Task<ServiceDetailedModel> CreateServiceAsync(ServiceDetailedModel serviceModel)
    {
        var newService = _mapper.Map<Service>(serviceModel);

        if (newService.CategoryId != 0)
        {
            var category = await _context.ServiceCategories
                             .Include(c => c.Services)
                             .AsNoTracking()
                             .FirstOrDefaultAsync(c => c.Id == newService.CategoryId);

            if (category is null)
            {
                throw new CategoryNotFoundException($"Category with id {newService.CategoryId} not found!");
            }
        }

        await _context.Services.AddAsync(newService);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<ServiceDetailedModel>(newService);

        return result;
    }

    public async Task<ServiceDetailedModel> UpdateServiceAsync(ServiceDetailedModel serviceModel)
    {
        var existingService = await _context.Services
                                            .Include(s => s.Category)
                                            .Include(s => s.Reservations)
                                            .Include(s => s.Masters)
                                            .FirstOrDefaultAsync(s => s.Id == serviceModel.Id);

        if (existingService is null)
        {
            throw new ServiceNotFoundException($"Service with id {serviceModel.Id} not found.");
        }

        _mapper.Map(serviceModel, existingService);

        existingService.Reservations = new List<Reservation>();

        foreach(var reservationId in serviceModel.ReservationIds)
        {
            var reservation = await _context.Reservations.FirstAsync(r => r.Id == reservationId);
            existingService.Reservations.Add(reservation);
        }

        existingService.Masters = new List<Master>();

        foreach (var masterId in serviceModel.MasterIds)
        {
            var master = await _context.Masters.FirstAsync(r => r.Id == masterId);
            existingService.Masters.Add(master);
        }

        if (serviceModel.CategoryId == 0)
        {
            existingService.CategoryId = null;
            existingService.Category = null;
        }
        else
        {
            var category = await _context.ServiceCategories.FirstAsync(c => c.Id == serviceModel.Id);
            existingService.CategoryId = serviceModel.CategoryId;
            existingService.Category = category;
        }

        await _context.SaveChangesAsync();

        var result = _mapper.Map<ServiceDetailedModel>(existingService);
        return result;
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
