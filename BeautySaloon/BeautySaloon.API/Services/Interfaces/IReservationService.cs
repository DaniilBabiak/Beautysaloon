using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;

namespace BeautySaloon.API.Services.Interfaces;

public interface IReservationService
{
    Task<Reservation> CreateReservationAsync(Reservation reservation);
    Task<List<GetAllReservationsResponse>> GetAllReservationsAsync();
    Task<List<Reservation>> GetAvailableReseravtionsByServiceId(int serviceId);
    Task<List<Reservation>> GetReservationsByServiceId(int serviceId);
}
