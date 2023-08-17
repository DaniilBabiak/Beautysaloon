namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class Service
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int? CategoryId { get; set; }
    public ServiceCategory? Category { get; set; }
    public TimeSpan? Duration { get; set; } = TimeSpan.FromHours(1);
    public List<Reservation>? Reservations { get; set; }
    public List<Master>? Masters { get; set; }
}
