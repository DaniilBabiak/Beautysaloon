namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class WorkingDay
{
    public int? WorkingDayId { get; set; }
    public int? ScheduleId { get; set; }
    public string Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
