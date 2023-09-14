using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Models.ReservationModels;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class ReservationService : IReservationService
{
    private readonly BeautySaloonContext _context;
    private readonly IMapper _mapper;

    public ReservationService(BeautySaloonContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReservationModel>> GetAllReservationsAsync()
    {
        var reservations = await _context.Reservations
            .Include(r => r.Service)
            .Include(r => r.Master)
            .ToListAsync();

        var result = _mapper.Map<List<ReservationModel>>(reservations);

        return result;
    }

    public async Task<List<ReservationModel>> GetReservationsByServiceId(int serviceId)
    {
        var reservations = await _context.Reservations
                                   .AsNoTracking()
                                   .Include(r => r.Service)
                                   .ToListAsync();

        var result = _mapper.Map<List<ReservationModel>>(reservations);

        return result;
    }

    public async Task<List<ReservationModel>> GetAvailableReseravtionsAsync(int serviceId, int masterId)
    {
        var service = await _context.Services
                                    .Include(s => s.Reservations)
                                    .Include(s => s.Masters)
                                    .Where(s => s.Id == serviceId)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

        if (service is null)
        {
            throw new ServiceNotFoundException($"Service with id {serviceId} not found.");
        }

        var availableReservations = await GetAvailableReservations(serviceId, masterId);

        var result = _mapper.Map<List<ReservationModel>>(availableReservations);

        return result;
    }

    public async Task<ReservationModel> CreateReservationAsync(ReservationModel reservationModel)
    {
        var availableReservations = await GetAvailableReservations(reservationModel.ServiceId, reservationModel.MasterId);

        var validReservation = availableReservations.FirstOrDefault(r => r.DateTime == reservationModel.DateTime);

        if (validReservation is null)
        {
            throw new ReservationNotAvailableException(reservationModel.ServiceId, reservationModel.DateTime);
        }

        if (string.IsNullOrEmpty(reservationModel.CustomerPhoneNumber))
        {
            throw new CustomerDoesntHavePhoneNumberException($"");
        }

        validReservation.CustomerPhoneNumber = reservationModel.CustomerPhoneNumber;

        var master = await _context.Masters.AsNoTracking().FirstOrDefaultAsync(c => c.Id == reservationModel.MasterId);

        if (master is null)
        {
            throw new MasterNotFoundException($"Master with id {reservationModel.MasterId} not found.");
        }

        validReservation.MasterId = reservationModel.MasterId;

        await _context.Reservations.AddAsync(validReservation);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<ReservationModel>(validReservation);

        return result;
    }

    private async Task<List<Reservation>> GetAvailableReservations(int serviceId, int masterId)
    {
        var master = await _context.Masters
                     .Include(m => m.Services)
                     .Include(m => m.Schedule)
                        .ThenInclude(s => s.WorkingDays)
                     .Include(m => m.Schedule)
                        .ThenInclude(s => s.DayOffs)
                     .Include(m => m.Reservations)
                     .Where(m => m.Id == masterId)
                     .AsNoTracking()
                     .FirstOrDefaultAsync();

        if (master is null)
        {
            return new List<Reservation>();
        }

        var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == serviceId);

        var existingReservationTimes = master.Reservations.Select(r => r.DateTime)// Получаем все резервации из мастеров
                                                          .ToList();

        var availableReservations = new List<Reservation>();

        DateTime currentDate = DateTime.Now.Date;
        DateTime endDate = currentDate.AddDays(13);

        TimeSpan duration = service.Duration.Value;

        if (master.Schedule is null)
        {
            return availableReservations;
        }

        foreach (var workingDay in master.Schedule.WorkingDays)
        {
            DateTime date = currentDate.Date;
            while (date <= endDate)
            {
                if (workingDay.Day == date.DayOfWeek.ToString())
                {
                    DateTime startDateTime = date.Date + workingDay.StartTime;
                    DateTime endDateTime = date.Date + workingDay.EndTime;

                    while (startDateTime.Add(duration) <= endDateTime)
                    {
                        DateTime reservationTime = startDateTime;
                        if (!existingReservationTimes.Any(r => r >= reservationTime && r < reservationTime.Add(duration)) && reservationTime >= DateTime.Now)
                        {
                            availableReservations.Add(new Reservation
                            {
                                ServiceId = serviceId,
                                Service = service,
                                DateTime = reservationTime,
                                Master = master
                            });
                        }

                        startDateTime = startDateTime.Add(duration);
                    }
                }

                date = date.AddDays(1);
            }
        }


        return availableReservations;
    }
}
