using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;

namespace BeautySaloon.API.Services.Interfaces;

public interface IMasterService
{
    Task AddDayOff(int masterId, DateTime date);
    Task<Master> CreateMasterAsync(Master master);
    Task DeleteMasterAsync(int masterId);
    Task<List<Master>> GetAllMastersAsync();
    Task<List<Master>> GetAllMastersByServiceIdAsync(int serviceId);
    Task<Master> GetMasterAsync(int masterId);
    Task<Master> UpdateMasterAsync(Master master);
}
