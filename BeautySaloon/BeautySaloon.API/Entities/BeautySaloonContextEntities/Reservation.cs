namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class Reservation
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public DateTime DateTime { get; set; }
}
