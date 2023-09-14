namespace BeautySaloon.API.Exceptions.NotFound;
[Serializable]
internal class ScheduleNotFoundException : Exception
{
    public ScheduleNotFoundException(string? message) : base(message)
    {
    }
}