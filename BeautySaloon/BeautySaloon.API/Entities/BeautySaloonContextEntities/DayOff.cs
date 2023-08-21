namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class DayOff
{
    public int? Id { get; set; }
    public int ScheduleId { get; set; }
    public Schedule? Schedule { get; set; }
    public DateTime Date { get; set; }
}
