using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Services.Interfaces;

public interface IReservationService
{
    Task<Reservation> CreateReservationAsync(Reservation reservation);
    Task<List<Reservation>> GetAvailableReseravtionsByServiceId(int serviceId);
    Task<List<Reservation>> GetReservationsByServiceId(int serviceId);
}
