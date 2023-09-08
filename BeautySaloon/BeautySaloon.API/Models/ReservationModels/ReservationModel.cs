namespace BeautySaloon.API.Models.ReservationModels;

public class ReservationModel
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public string ServiceName { get; set; }
    public DateTime DateTime { get; set; }
    public string CustomerPhoneNumber { get; set; }
    public int MasterId { get; set; }
    public string MasterName { get; set; }
}