using System.Runtime.Serialization;

namespace BeautySaloon.API.Exceptions.NotFound;
[Serializable]
internal class MasterNotFoundException : NotFoundException
{
    public MasterNotFoundException(string message) : base(message)
    {
    }
}