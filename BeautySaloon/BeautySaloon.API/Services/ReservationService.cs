using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class ReservationService : IReservationService
{
    private readonly BeautySaloonContext _context;
    private readonly IMasterService _masterService;
    public ReservationService(BeautySaloonContext context, IMasterService masterService)
    {
        _context = context;
        _masterService = masterService;
    }

    public async Task<List<GetAllReservationsResponse>> GetAllReservationsAsync()
    {
        var services = await _context.Services
                                     .Include(s => s.Reservations)
                                     .ThenInclude(r => r.Customer)
                                     .ToListAsync();

        var result = new List<GetAllReservationsResponse>();

        foreach (var service in services)
        {
            result.Add(new GetAllReservationsResponse
            {
                ServiceId = service.Id.Value,
                ServiceName = service.Name,
                Reservations = service.Reservations
            });
        }

        return result;
    }

    public async Task<List<Reservation>> GetReservationsByServiceId(int serviceId)
    {
        var result = await _context.Reservations
                                   .AsNoTracking()
                                   .Include(r => r.Service)
                                   .Include(r => r.Customer)
                                   .ToListAsync();

        return result;
    }

    public async Task<List<Reservation>> GetAvailableReseravtionsByServiceId(int serviceId)
    {
        var service = await _context.Services
                                    .AsNoTracking()
                                    .Include(s => s.Reservations)
                                    .Include(s => s.Masters)
                                    .Where(s => s.Id == serviceId).FirstOrDefaultAsync();

        if (service is null)
        {
            throw new ServiceNotFoundException($"Service with id {serviceId} not found.");
        }

        var availableReservations = await GetAvailableReservations(serviceId);

        return availableReservations;
    }

    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        var availableReservations = await GetAvailableReseravtionsByServiceId(reservation.ServiceId);
        var validReservation = availableReservations.FirstOrDefault(r => r.DateTime == reservation.DateTime);

        if (validReservation is null)
        {
            throw new ReservationNotAvailableException(reservation.ServiceId, reservation.DateTime);
        }

        var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == reservation.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException($"Customer with id {reservation.CustomerId} not found.");
        }

        var master = await _context.Masters.AsNoTracking().FirstOrDefaultAsync(c => c.Id == reservation.MasterId);

        if (master is null)
        {
            throw new MasterNotFoundException($"Master with id {reservation.MasterId} not found.");
        }

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();

        return reservation;
    }

    private async Task<List<Reservation>> GetAvailableReservations(int serviceId)
    {
        var masters = await _masterService.GetAllMastersByServiceIdAsync(serviceId);

        if (!masters.Any())
        {
            return new List<Reservation>();
        }

        var service = masters.First().Services.First(s => s.Id == serviceId);

        var existingReservationTimes = masters.SelectMany(m => m.Reservations) // Получаем все резервации из мастеров
                                              .Select(r => r.DateTime)
                                              .ToList();
            
        var availableReservations = new List<Reservation>();

        DateTime currentDate = DateTime.Now.Date;
        DateTime endDate = currentDate.AddDays(13);

        TimeSpan duration = service.Duration.Value;

        foreach (var master in masters)
        {
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
                                    DateTime = reservationTime
                                });
                            }

                            startDateTime = startDateTime.Add(duration);
                        }
                    }

                    date = date.AddDays(1);
                }
            }
        }

        return availableReservations;
    }
}
