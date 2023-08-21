using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Models;

public class CreateMasterModel
{
    public string Name { get; set; }
    public List<int> ServiceIds { get; set; }
}
