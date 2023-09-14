using BeautySaloon.API.Models.ReservationModels;

namespace BeautySaloon.API.Services.Interfaces;

public interface IReservationService
{
    Task<ReservationModel> CreateReservationAsync(ReservationModel reservationModel);
    Task<List<ReservationModel>> GetAllReservationsAsync();
    Task<List<ReservationModel>> GetAvailableReseravtionsAsync(int serviceId, int masterId);
    Task<List<ReservationModel>> GetReservationsByServiceId(int serviceId);
}
