using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;

namespace BeautySaloon.API.Services.Interfaces;

public interface IReservationService
{
    Task<Reservation> CreateAnonymousReservationAsync(CreateAnonymousReservationRequest createAnonymousReservationRequest);
    Task<Reservation> CreateReservationAsync(CreateReservationRequest reservation, string customerId);
    Task<List<Reservation>> GetAllReservationsAsync();
    Task<List<Reservation>> GetAvailableReseravtionsByServiceId(int serviceId);
    Task<List<Reservation>> GetReservationsByServiceId(int serviceId);
}
