namespace BeautySaloon.API.Models.ServiceModels;

public class ServiceDetailedModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public TimeSpan Duration { get; set; }
    public List<int> ReservationIds { get; set; }
    public List<int> MasterIds { get; set; }
}
