namespace BeautySaloon.API.Exceptions.NotFound;

public class ServiceNotFoundException : NotFoundException
{
    public ServiceNotFoundException(string message) : base(message)
    {
    }
}
