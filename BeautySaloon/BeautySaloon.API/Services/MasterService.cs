using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Models.MasterModels;
using BeautySaloon.API.Models.ScheduleModels;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class MasterService : IMasterService
{
    private readonly BeautySaloonContext _context;
    private readonly IMapper _mapper;
    public MasterService(BeautySaloonContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    #region master
    public async Task<List<MasterModel>> GetAllMastersAsync()
    {
        var masters = await _context.Masters
                             .Include(m => m.Services)
                             .Include(m => m.Schedule)
                                .ThenInclude(s => s.WorkingDays)
                             .Include(m => m.Schedule)
                                .ThenInclude(s => s.DayOffs)
                             .Include(m => m.Reservations)
                             .AsNoTracking()
                             .ToListAsync();

        var result = _mapper.Map<List<MasterModel>>(masters);

        return result;
    }
    public async Task<MasterDetailedModel> GetMasterAsync(int masterId)
    {
        var master = await _context.Masters
                             .Include(m => m.Services)
                             .Include(m => m.Schedule)
                                .ThenInclude(s => s.WorkingDays)
                             .Include(m => m.Schedule)
                                .ThenInclude(s => s.DayOffs)
                             .Include(m => m.Reservations)
                             .AsNoTracking()
                             .FirstOrDefaultAsync(m => m.Id == masterId);

        if (master is null)
        {
            throw new MasterNotFoundException($"Master with id {masterId} not found.");
        }

        var result = _mapper.Map<MasterDetailedModel>(master);

        return result;
    }
    public async Task<MasterDetailedModel> CreateMasterAsync(MasterDetailedModel masterDetailedModel)
    {
        var newMaster = _mapper.Map<Master>(masterDetailedModel);

        newMaster.Services = new List<Service>();

        foreach (var serviceId in masterDetailedModel.ServiceIds)
        {
            var service = await _context.Services.Include(s => s.Masters).FirstOrDefaultAsync(s => s.Id == serviceId);
            if (service is null)
            {
                throw new ServiceNotFoundException($"Service with id {serviceId} not found.");
            }

            newMaster.Services.Add(service);
        }
        await _context.AddAsync(newMaster);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<MasterDetailedModel>(newMaster);

        return result;
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
    public async Task<MasterDetailedModel> UpdateMasterAsync(MasterDetailedModel updateMasterModel)
    {
        var existingMaster = await _context.Masters
                                           .Include(m => m.Services)
                                           .Include(m => m.Reservations)
                                           .SingleOrDefaultAsync(m => m.Id == updateMasterModel.Id);

        if (existingMaster is null)
        {
            throw new MasterNotFoundException($"Master with id {updateMasterModel.Id} not found.");
        }

        _mapper.Map(updateMasterModel, existingMaster);

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

        existingMaster.Reservations = new List<Reservation>();

        foreach (var reservationId in updateMasterModel.ReservationIds)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(s => s.Id == reservationId);
            if (reservation is null)
            {
                throw new ReservationNotFoundException($"Reservation with id {reservationId} not found.");
            }

            existingMaster.Reservations.Add(reservation);
        }

        await _context.SaveChangesAsync();

        var result = _mapper.Map<MasterDetailedModel>(existingMaster);

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
                     .AsNoTracking()
                     .ToListAsync();

        return result;
    }
    #endregion
    #region schedule
    public async Task<ScheduleModel> GetScheduleByMasterIdAsync(int masterId)
    {
        var master = await _context.Masters
                             .AsNoTracking()
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

        var result = _mapper.Map<ScheduleModel>(master.Schedule);

        return result;
    }
    public async Task<ScheduleModel> CreateScheduleAsync(ScheduleModel scheduleModel)
    {
        var newSchedule = _mapper.Map<Schedule>(scheduleModel);

        var master = await _context.Masters.FirstOrDefaultAsync(m => m.Id == newSchedule.MasterId);

        if (master is null)
        {
            throw new MasterNotFoundException($"Master with id {newSchedule.MasterId} not found.");
        }

        await _context.Schedules.AddAsync(newSchedule);

        await _context.SaveChangesAsync();

        var result = _mapper.Map<ScheduleModel>(newSchedule);

        return result;
    }
    public async Task<ScheduleModel> UpdateScheduleAsync(ScheduleModel scheduleModel)
    {
        var updateScheduleTransaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var existingSchedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == scheduleModel.Id);

            if (existingSchedule is null)
            {
                throw new ScheduleNotFoundException($"Schedule with id {scheduleModel.Id} not found.");
            }

            if (existingSchedule.Master is null)
            {
                throw new MasterNotFoundException($"Master with id {scheduleModel.MasterId}");
            }

            var newSchedule = _mapper.Map<Schedule>(scheduleModel);

            _context.Schedules.Remove(existingSchedule);
            await _context.AddAsync(newSchedule);

            //foreach(var workingDay in newSchedule.WorkingDays)
            //{

            //}

            await _context.SaveChangesAsync();

            var result = _mapper.Map<ScheduleModel>(newSchedule);

            await updateScheduleTransaction.CommitAsync();

            return result;
        }
        catch
        {
            await updateScheduleTransaction.RollbackAsync();
            throw;
        }
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
    public async Task<ScheduleModel> GetScheduleAsync(int scheduleId)
    {
        var schedule = await _context.Schedules
                                        .Include(s => s.WorkingDays)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(s => s.Id == scheduleId);

        if (schedule is null)
        {
            throw new ScheduleNotFoundException($"Shedule with id {scheduleId} not found.");
        }

        var result = _mapper.Map<ScheduleModel>(schedule);
        return result;
    }
    #endregion

    //private async Task<bool> CanChangeMasterWorkingHours(int masterId, List<WorkingDay> newWorkingDays)
    //{
    //    // Получите все записи для данного мастера
    //    var masterReservations = await _context.Reservations
    //                                           .AsNoTracking()
    //                                           .Include(r => r.Service)
    //                                           .ToListAsync();

    //    foreach (var reservation in masterReservations)
    //    {
    //        foreach (var newWorkingDay in newWorkingDays)
    //        {
    //            if (reservation.DateTime.DayOfWeek.ToString() != newWorkingDay.Day)
    //            {
    //                continue;
    //            }

    //            var reservationStartTime = reservation.DateTime.TimeOfDay;
    //            var reservationEndTime = reservationStartTime.Add(reservation.Service.Duration.Value);

    //            if (newWorkingDay.StartTime >= reservationStartTime && newWorkingDay.EndTime)
    //            {
    //            }
    //        }

    //        foreach (var newWorkingDay in newWorkingDays)
    //        {
    //            // Проверьте, есть ли пересечение с существующими записями (резервациями)
    //            foreach (var reservation in masterReservations)
    //            {
    //                if (reservation.DateTime.DayOfWeek.ToString() == newWorkingDay.Day)
    //                {
    //                    var reservationStartTime = reservation.DateTime.TimeOfDay;
    //                    var reservationEndTime = reservationStartTime.Add(reservation.Service.Duration.Value);

    //                    if ((newWorkingDay.StartTime >= reservationStartTime && newWorkingDay.StartTime < reservationEndTime) ||
    //                        (newWorkingDay.EndTime > reservationStartTime && newWorkingDay.EndTime <= reservationEndTime))
    //                    {
    //                        // Если есть пересечение с существующими записями, верните false
    //                        return false;
    //                    }
    //                }
    //            }
    //        }

    //        // Если не найдено пересечений, верните true
    //        return true;
    //    }
    //}

}
