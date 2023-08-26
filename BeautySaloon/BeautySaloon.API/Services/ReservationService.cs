using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Entities.Contexts;
using BeautySaloon.API.Exceptions;
using BeautySaloon.API.Exceptions.NotFound;
using BeautySaloon.API.Helpers;
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

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        var reservations = await _context.Reservations
            .Include(r => r.Service)
            .Include(r => r.Master)
            .ToListAsync();

        return reservations;
    }

    public async Task<List<Reservation>> GetReservationsByServiceId(int serviceId)
    {
        var result = await _context.Reservations
                                   .AsNoTracking()
                                   .Include(r => r.Service)
                                   .ToListAsync();

        return result;
    }

    public async Task<List<Reservation>> GetAvailableReseravtionsByServiceId(int serviceId)
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

        var availableReservations = await GetAvailableReservations(serviceId);

        return availableReservations;
    }

    public async Task<Reservation> CreateReservationAsync(CreateReservationRequest createReservationRequest, string customerId)
    {
        var availableReservations = await GetAvailableReseravtionsByServiceId(createReservationRequest.ServiceId);
        var validReservation = availableReservations.FirstOrDefault(r => r.DateTime == createReservationRequest.DateTime);

        if (validReservation is null)
        {
            throw new ReservationNotAvailableException(createReservationRequest.ServiceId, createReservationRequest.DateTime);
        }

        var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException($"Customer with id {customerId} not found.");
        }

        if (string.IsNullOrEmpty(customer.PhoneNumber))
        {
            throw new CustomerDoesntHavePhoneNumberException($"");
        }

        validReservation.CustomerPhoneNumber = customer.PhoneNumber;

        var master = await _context.Masters.AsNoTracking().FirstOrDefaultAsync(c => c.Id == createReservationRequest.MasterId);

        if (master is null)
        {
            throw new MasterNotFoundException($"Master with id {createReservationRequest.MasterId} not found.");
        }

        validReservation.Master = master;

        await _context.Reservations.AddAsync(validReservation);
        await _context.SaveChangesAsync();

        return validReservation;
    }

    public async Task<Reservation> CreateAnonymousReservationAsync(CreateAnonymousReservationRequest createAnonymousReservationRequest)
    {
        var availableReservations = await GetAvailableReseravtionsByServiceId(createAnonymousReservationRequest.ServiceId);
        var validReservation = availableReservations.FirstOrDefault(r => r.DateTime == createAnonymousReservationRequest.DateTime);

        if (validReservation is null)
        {
            throw new ReservationNotAvailableException(createAnonymousReservationRequest.ServiceId, createAnonymousReservationRequest.DateTime);
        }

        createAnonymousReservationRequest.PhoneNumber = PhoneNumberHelper
                                                           .ConvertToE164PhoneNumber(createAnonymousReservationRequest.PhoneNumber);

        if (string.IsNullOrEmpty(createAnonymousReservationRequest.PhoneNumber))
        {
            throw new Exception($"");
        }

        validReservation.CustomerPhoneNumber = createAnonymousReservationRequest.PhoneNumber;

        var master = await _masterService.GetMasterAsync(createAnonymousReservationRequest.MasterId);

        if (master is null)
        {
            throw new MasterNotFoundException($"Master with id {createAnonymousReservationRequest.MasterId} not found.");
        }

        var newReservation = new Reservation()
        {
            MasterId = createAnonymousReservationRequest.MasterId,
            ServiceId = createAnonymousReservationRequest.ServiceId,
            DateTime = createAnonymousReservationRequest.DateTime,
            CustomerPhoneNumber = createAnonymousReservationRequest.PhoneNumber
        };

        await _context.Reservations.AddAsync(newReservation);
        await _context.SaveChangesAsync();

        return newReservation;
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
            if (master.Schedule is null)
            {
                continue;
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
        }

        return availableReservations;
    }
}
