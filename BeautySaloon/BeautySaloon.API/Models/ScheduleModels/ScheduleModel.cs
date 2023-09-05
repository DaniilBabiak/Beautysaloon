namespace BeautySaloon.API.Models.ScheduleModels;

public class ScheduleModel
{
    public int Id { get; set; }
    public int MasterId { get; set; }
    public List<WorkingDayModel> Workingdays { get; set; }
}
