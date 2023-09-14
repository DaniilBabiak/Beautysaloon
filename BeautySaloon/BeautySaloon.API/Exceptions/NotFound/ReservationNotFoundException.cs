namespace BeautySaloon.API.Exceptions.NotFound;
[Serializable]
internal class ReservationNotFoundException : NotFoundException
{
    public ReservationNotFoundException(string message) : base(message)
    {
    }
}