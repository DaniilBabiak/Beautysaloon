namespace BeautySaloon.API.Models;

public class CreateAnonymousReservationRequest
{
    public int ServiceId { get; set; }
    public DateTime DateTime { get; set; }
    public int MasterId { get; set; }
    public string PhoneNumber { get; set; }
}
