namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class Service
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }
    public ServiceCategory? Category { get; set; }
    public TimeSpan StartTime { get; set; } = TimeSpan.FromHours(9);
    public TimeSpan EndTime { get; set; } = TimeSpan.FromHours(17);
    public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1);
    public List<Reservation>? Reservations { get; set; }
}
