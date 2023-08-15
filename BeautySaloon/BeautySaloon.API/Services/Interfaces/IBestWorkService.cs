using BeautySaloon.API.Entities.BeautySaloonContextEntities;

namespace BeautySaloon.API.Services.Interfaces;

public interface IBestWorkService
{
    Task<BestWork> CreateBestWorkAsync(BestWork bestWork);
    Task DeleteBestWorkAsync(int id);
    Task<List<BestWork>> GetAllBestWorksAsync();
}
