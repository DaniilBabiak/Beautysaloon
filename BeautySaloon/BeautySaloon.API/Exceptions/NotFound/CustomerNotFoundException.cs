using System.Runtime.Serialization;

namespace BeautySaloon.API.Exceptions.NotFound;
[Serializable]
internal class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(string message) : base(message)
    {
    }
}