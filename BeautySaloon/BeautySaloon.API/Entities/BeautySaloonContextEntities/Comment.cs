namespace BeautySaloon.API.Entities.BeautySaloonContextEntities;

public class Comment
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public int Rate { get; set; }
    public string Message { get; set; }
}
