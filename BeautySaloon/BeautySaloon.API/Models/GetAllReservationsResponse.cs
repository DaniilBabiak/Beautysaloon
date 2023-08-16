using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Models;

public class GetAllReservationsResponse
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; }
    public List<Reservation> Reservations { get; set; }
}
