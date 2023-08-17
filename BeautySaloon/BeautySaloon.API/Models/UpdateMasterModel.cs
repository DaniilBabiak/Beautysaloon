using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Models;

public class UpdateMasterModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int>? ServiceIds { get; set; }
    public List<WorkingDay>? WorkingDays { get; set; }
    public List<int>? ReservationIds { get; set; }
    public List<DayOff> DayOffs { get; set; }
}
