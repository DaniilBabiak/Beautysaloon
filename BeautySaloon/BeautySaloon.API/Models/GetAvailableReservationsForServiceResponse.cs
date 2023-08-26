using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Models;

public class GetAvailableReservationsForServiceResponse
{
    public Service Service { get; set; }
    public DateTime AvailableTime { get; set; }
    public Master Master { get; set; }
}
