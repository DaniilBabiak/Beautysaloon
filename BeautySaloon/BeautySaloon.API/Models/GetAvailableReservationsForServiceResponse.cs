using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Models;

public class GetAvailableReservationsForServiceResponse
{
    public Service Service { get; set; }
    public IEnumerable<DateTime> AvailableReservations { get; set; } = new List<DateTime>();
}
