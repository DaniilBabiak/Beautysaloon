using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;

namespace BeautySaloon.API.Services.Interfaces;

public interface IMasterService
{
    Task AddDayOff(int masterId, DateTime date);
    Task<Master> CreateMasterAsync(CreateMasterModel master);
    Task<Schedule> CreateScheduleAsync(int masterId, Schedule schedule);
    Task DeleteMasterAsync(int masterId);
    Task<List<Master>> GetAllMastersAsync();
    Task<List<Master>> GetAllMastersByServiceIdAsync(int serviceId);
    Task<Master> GetMasterAsync(int masterId);
    Task<Schedule> GetScheduleAsync(int masterId);
    Task<Master> UpdateMasterAsync(UpdateMasterModel master);
    Task<Schedule> UpdateScheduleAsync(Schedule schedule);
}
