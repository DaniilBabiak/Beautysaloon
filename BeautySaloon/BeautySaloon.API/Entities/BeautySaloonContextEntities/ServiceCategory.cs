namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class ServiceCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageBucket { get; set; }
    public string ImageFileName { get; set; }
    public List<Service>? Services { get; set; }
}
