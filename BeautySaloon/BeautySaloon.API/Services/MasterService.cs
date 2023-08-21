using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace BeautySaloon.API.Services;

public class MasterService : IMasterService
{
    private readonly BeautySaloonContext _context;
    private readonly IServiceService _serviceService;
    public MasterService(BeautySaloonContext context, IServiceService serviceService)
    {
        _context = context;
        _serviceService = serviceService;
    }

    public async Task<Schedule> GetScheduleAsync(int masterId)
    {
        var master = await _context.Masters
                             .Include(m => m.Schedule)
                                .ThenInclude(s => s.WorkingDays)
                             .Include(m => m.Schedule)
                                .ThenInclude(s => s.DayOffs)
                             .FirstOrDefaultAsync(m => m.Id == masterId);

        if (master is null)
        {
            throw new MasterNotFoundException($"Master with id {masterId} not found.");
        }

        if (master.Schedule is null)
        {
            throw new ScheduleNotFoundException($"Shedule for master with id {masterId} not found.");
        }

        return master.Schedule;
    }

    public async Task<Schedule> CreateScheduleAsync(int masterId, Schedule schedule)
    {
        var master = await _context.Masters.FirstOrDefaultAsync(m => m.Id == masterId);

        if (master is null)
        {
            throw new MasterNotFoundException($"Master with id {masterId} not found.");
        }

        var newSchedule = new Schedule
        {
            MasterId = masterId,
            WorkingDays = schedule.WorkingDays,
            DayOffs = schedule.DayOffs,
        };

        master.Schedule = newSchedule;


        await _context.SaveChangesAsync();

        return newSchedule;
    }
    public async Task<Schedule> UpdateScheduleAsync(Schedule schedule)
    {
        var existingSchedule = await _context.Schedules
                                       .AsNoTracking()
                                       .Include(s => s.Master)
                                       .FirstOrDefaultAsync(s => s.Id == schedule.Id);

        if (existingSchedule is null)
        {
            throw new ScheduleNotFoundException($"Schedule with id {schedule.Id} not found.");
        }

        if (existingSchedule.Master is null)
        {
            throw new MasterNotFoundException($"Master with id {schedule.Master.Id}");
        }

        _context.Schedules.Update(schedule);
        await _context.SaveChangesAsync();

        return schedule;
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

    public async Task<Master> CreateMasterAsync(CreateMasterModel createMasterModel)
    {
        var newMaster = new Master()
        {
            Name = createMasterModel.Name
        };

        await _context.Masters.AddAsync(newMaster);

        newMaster.Services = new List<Service>();

        foreach (var serviceId in createMasterModel.ServiceIds)
        {
            var service = await _context.Services.Include(s => s.Masters).FirstOrDefaultAsync(s => s.Id == serviceId);
            if (service is null)
            {
                throw new ServiceNotFoundException($"Service with id {serviceId} not found.");
            }

            service.Masters = service.Masters ?? new List<Master>();

            service.Masters.Add(newMaster);
            newMaster.Services.Add(service);
        }

        await _context.SaveChangesAsync();

        return newMaster;
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

    public async Task<Master> UpdateMasterAsync(UpdateMasterModel updateMasterModel)
    {
        var existingMaster = await _context.Masters
                                           .Include(m => m.Services)
                                           .SingleOrDefaultAsync(m => m.Id == updateMasterModel.Id);

        if (existingMaster is null)
        {
            throw new MasterNotFoundException($"Master with id {updateMasterModel.Id} not found.");
        }

        existingMaster.Services = new List<Service>();

        foreach (var serviceId in updateMasterModel.ServiceIds)
        {
            var service = await _context.Services.Include(s => s.Masters).FirstOrDefaultAsync(s => s.Id == serviceId);
            if (service is null)
            {
                throw new ServiceNotFoundException($"Service with id {serviceId} not found.");
            }

            service.Masters = service.Masters ?? new List<Master>();

            if (!service.Masters.Select(m => m.Id).Contains(existingMaster.Id))
            {
                service.Masters.Add(existingMaster);
            }

            existingMaster.Services.Add(service);
        }
        
        await _context.SaveChangesAsync();

        return existingMaster;
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
        if (master.Services is not null)
        {
            foreach (var masterService in master.Services)
            {
                var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == masterService.Id);

                if (service is null)
                {
                    throw new ServiceNotFoundException($"Service with id {masterService.Id} not found");
                }
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
