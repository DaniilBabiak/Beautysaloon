using BeautySaloon.API.Models.BestWorkModels;

namespace BeautySaloon.API.Services.Interfaces;

public interface IBestWorkService
{
    Task<BestWorkModel> CreateBestWorkAsync(BestWorkModel bestWork);
    Task DeleteBestWorkAsync(int id);
    Task<List<BestWorkModel>> GetAllBestWorksAsync();
}
