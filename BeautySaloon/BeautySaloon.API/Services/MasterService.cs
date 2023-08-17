using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class MasterService : IMasterService
{
    private readonly BeautySaloonContext _context;

    public MasterService(BeautySaloonContext context)
    {
        _context = context;
    }

    public async Task<List<Master>> GetAllMastersAsync()
    {
        var result = await _context.Masters
                             .Include(m => m.Services)
                             .Include(m => m.Schedule)
                             .ThenInclude(s => s.WorkingDays)
                             .Include(m => m.Schedule)
                             .ThenInclude(s => s.DayOffs)
                             .Include(m => m.Reservations)
                             .ToListAsync();

        return result;
    }

    public async Task<List<Master>> GetAllMastersByServiceIdAsync(int serviceId)
    {
        var result = await _context.Masters
                     .Include(m => m.Services)
                     .Include(m => m.Schedule)
                        .ThenInclude(s => s.WorkingDays)
                     .Include(m => m.Schedule)
                        .ThenInclude(s => s.DayOffs)
                     .Include(m => m.Reservations)
                     .Where(m => m.Services.Select(s => s.Id).Contains(serviceId))
                     .ToListAsync();

        return result;
    }

    public async Task<Master> GetMasterAsync(int masterId)
    {
        var result = await _context.Masters
                             .Include(m => m.Services)
                             .Include(m => m.Schedule)
                                .ThenInclude(s => s.WorkingDays)
                             .Include(m => m.Schedule)
                                .ThenInclude(s => s.DayOffs)
                             .Include(m => m.Reservations)
                             .FirstOrDefaultAsync(m => m.Id == masterId);

        if (result is null)
        {
            throw new MasterNotFoundException($"Master with id {masterId} not found.");
        }

        return result;
    }

    public async Task<Master> CreateMasterAsync(Master master)
    {
        await ValidateMaster(master);

        if (master.Services is not null && master.Services.Any())
        {
            var services = await _context.Services.Where(s => master.Services.Select(s => s.Id).Contains(s.Id)).ToListAsync();
            master.Services = services;
        }

        if (master.Reservations is not null && master.Reservations.Any())
        {
            var reservations = await _context.Reservations
                             .Where(r => master.Reservations.Select(r => r.Id).Contains(r.Id)).ToListAsync();
            master.Reservations = reservations;
        }

        await _context.Masters.AddAsync(master);
        await _context.SaveChangesAsync();

        return master;
    }

    public async Task DeleteMasterAsync(int masterId)
    {
        var masterToRemove = await _context.Masters.FirstOrDefaultAsync(master => master.Id == masterId);
        if (masterToRemove is null)
        {
            throw new MasterNotFoundException($"Master with id {masterId} not found.");
        }

        _context.Masters.Remove(masterToRemove);
        await _context.SaveChangesAsync();
    }

    public async Task<Master> UpdateMasterAsync(Master master)
    {
        var existingMaster = await _context.Masters.AsNoTracking().SingleOrDefaultAsync(m => m.Id == master.Id);

        if (existingMaster is null)
        {
            throw new MasterNotFoundException($"Master with id {master.Id} not found.");
        }

        await ValidateMaster(master);

        if (master.Services is not null && master.Services.Any())
        {
            var services = await _context.Services.Where(s => master.Services.Select(s => s.Id).Contains(s.Id)).ToListAsync();
            master.Services = services;
        }

        if (master.Reservations is not null && master.Reservations.Any())
        {
            var reservations = await _context.Reservations
                             .Where(r => master.Reservations.Select(r => r.Id).Contains(r.Id)).ToListAsync();
            master.Reservations = reservations;
        }

        _context.Masters.Update(master);
        await _context.SaveChangesAsync();

        return master;
    }

    public async Task AddDayOff(int masterId, DateTime date)
    {
        var masterToUpdate = await _context.Masters
                                           .Include(m => m.Schedule)
                                           .ThenInclude(s => s.DayOffs)
                                           .FirstOrDefaultAsync(master => master.Id == masterId);

        if (masterToUpdate is null)
        {
            throw new MasterNotFoundException($"Master with id {masterId} not found.");
        }

        if (masterToUpdate.Schedule.DayOffs == null)
        {
            masterToUpdate.Schedule.DayOffs = new List<DayOff>();
        }

        masterToUpdate.Schedule.DayOffs.Add(new DayOff { Date = date });
        await _context.SaveChangesAsync();
    }

    private async Task ValidateMaster(Master master)
    {


        foreach (var masterService in master.Services)
        {
            var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == masterService.Id);

            if (service is null)
            {
                throw new ServiceNotFoundException($"Service with id {masterService.Id} not found");
            }
        }

        if (master.Reservations is not null)
        {
            foreach (var masterReservation in master.Reservations)
            {
                var reservation = await _context.Reservations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == masterReservation.Id);

                if (reservation is null)
                {
                    throw new ReservationNotFoundException($"Reservation with id {masterReservation.Id} not found");
                }
            }
        }
    }
}
