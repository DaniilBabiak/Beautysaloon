namespace BeautySaloon.API.Models.MasterModels;

public class MasterDetailedModel
{
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public string Name { get; set; }
    public List<int> ServiceIds { get; set; }
    public List<int> ReservationIds { get; set; }
}