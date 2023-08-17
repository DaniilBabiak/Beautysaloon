namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class Master
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public List<Service>? Services { get; set; }
    public int? ScheduleId { get; set; }
    public Schedule? Schedule { get; set; }
    public List<Reservation>? Reservations { get; set; }
}
