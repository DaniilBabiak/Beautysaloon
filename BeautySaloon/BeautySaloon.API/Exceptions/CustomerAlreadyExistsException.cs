namespace BeautySaloon.API.Exceptions;
[Serializable]
public class CustomerAlreadyExistsException : Exception
{
    public CustomerAlreadyExistsException(string? message) : base(message)
    {
    }
}