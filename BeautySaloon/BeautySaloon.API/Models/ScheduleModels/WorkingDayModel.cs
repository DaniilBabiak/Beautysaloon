namespace BeautySaloon.API.Models.ScheduleModels;

public class WorkingDayModel
{
    public string Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
