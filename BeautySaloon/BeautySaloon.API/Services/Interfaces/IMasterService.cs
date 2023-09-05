using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.MasterModels;
using BeautySaloon.API.Models.ScheduleModels;

namespace BeautySaloon.API.Services.Interfaces;

public interface IMasterService
{
    Task AddDayOff(int masterId, DateTime date);
    Task<MasterDetailedModel> CreateMasterAsync(MasterDetailedModel master);
    Task<ScheduleModel> CreateScheduleAsync(ScheduleModel createScheduleModel);
    Task DeleteMasterAsync(int masterId);
    Task<List<MasterModel>> GetAllMastersAsync();
    Task<List<Master>> GetAllMastersByServiceIdAsync(int serviceId);
    Task<MasterDetailedModel> GetMasterAsync(int masterId);
    Task<ScheduleModel> GetScheduleAsync(int scheduleId);
    Task<ScheduleModel> GetScheduleByMasterIdAsync(int masterId);
    Task<MasterDetailedModel> UpdateMasterAsync(MasterDetailedModel master);
    Task<ScheduleModel> UpdateScheduleAsync(ScheduleModel scheduleModel);
}
