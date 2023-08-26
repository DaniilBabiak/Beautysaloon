using System.Runtime.Serialization;

namespace BeautySaloon.API.Exceptions;
[Serializable]
internal class CustomerDoesntHavePhoneNumberException : Exception
{
    public CustomerDoesntHavePhoneNumberException(string? message) : base(message)
    {
    }
}