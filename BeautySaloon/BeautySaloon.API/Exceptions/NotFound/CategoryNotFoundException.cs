namespace BeautySaloon.API.Exceptions.NotFound;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(string message) : base(message)
    {
    }
}
