namespace BeautySaloon.API.Exceptions;

public class ServiceNotFoundException : NotFoundException
{
    public ServiceNotFoundException(string message) : base(message)
    {
    }
}
