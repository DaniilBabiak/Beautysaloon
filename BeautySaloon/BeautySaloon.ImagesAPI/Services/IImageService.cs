using BeautySaloon.ImagesAPI.Models;

namespace BeautySaloon.ImagesAPI.Services;

public interface IImageService
{
    Task<byte[]> GetImageAsync(MinioLocation location);
    Task<MinioLocation> SaveImageAsync(IFormFile file, string bucketName);
    Task DeleteImageAsync(MinioLocation location);
    Task<List<MinioLocation>> GetAllImagesAsync();
}
