namespace BeautySaloon.ImagesAPI.Exceptions.NotFoundExceptions;

public class FileNotFoundException : NotFoundException
{
    public FileNotFoundException(string message) : base(message)
    {
    }
}
