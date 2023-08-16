using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.API.Services;

public class ReservationService : IReservationService
{
    private readonly BeautySaloonContext _context;

    public ReservationService(BeautySaloonContext context)
    {
        _context = context;
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
                                    .Where(s => s.Id == serviceId).FirstOrDefaultAsync();

        if (service is null)
        {
            throw new ServiceNotFoundException($"Service with id {serviceId} not found.");
        }

        var availableReservations = GetAvailableReservations(serviceId, service);

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

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();

        return reservation;
    }

    private List<Reservation> GetAvailableReservations(int serviceId, Service service)
    {
        var existingReservationTimes = service.Reservations.Where(r => r.ServiceId == serviceId)
                                                   .Select(r => r.DateTime)
                                                   .ToList();

        var availableReservations = new List<Reservation>();

        DateTime currentDate = DateTime.Now.Date; // Сегодняшняя дата без времени
        DateTime endDate = currentDate.AddDays(13); // Дата, на 2 недели вперед

        TimeSpan startTime = service.StartTime;
        TimeSpan endTime = service.EndTime;
        TimeSpan duration = service.Duration;

        for (DateTime date = currentDate; date <= endDate; date = date.AddDays(1))
        {
            DateTime startDateTime = date.Add(startTime);
            DateTime endDateTime = date.Add(endTime);

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

        return availableReservations;
    }
}
