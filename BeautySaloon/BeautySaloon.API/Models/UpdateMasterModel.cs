using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Models;

public class UpdateMasterModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> ServiceIds { get; set; }
}
