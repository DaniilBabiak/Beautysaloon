namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class Reservation
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public DateTime DateTime { get; set; }
    public string CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int MasterId { get; set; }
    public Master? Master { get; set; }
}
