namespace BeautySaloon.API.Exceptions;

public class ReservationNotAvailableException : Exception
{
    public int ServiceId { get; init; }
    public DateTime DateTime { get; init; }

    public ReservationNotAvailableException(int serviceId, DateTime dateTime)
        : base($"{dateTime} is not available reservation dateTime for service {serviceId}")
    {
        ServiceId = serviceId;
        DateTime = dateTime;
    }
}
