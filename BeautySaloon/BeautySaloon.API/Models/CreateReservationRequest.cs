using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Models;

public class CreateReservationRequest
{
    public int ServiceId { get; set; }
    public DateTime DateTime { get; set; }
    public int MasterId { get; set; }
}
