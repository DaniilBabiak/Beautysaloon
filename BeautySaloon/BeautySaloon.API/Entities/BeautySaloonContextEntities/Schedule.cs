namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class Schedule
{
    public int? Id { get; set; }
    public int? MasterId { get; set; }
    public Master? Master { get; set; }
    public List<WorkingDay>? WorkingDays { get; set; }
    public List<DayOff>? DayOffs { get; set; }
}