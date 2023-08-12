using BeautySaloon.ImagesAPI.Models;

namespace BeautySaloon.ImagesAPI.Services;

public interface IImageService
{
    public byte[] GetImageAsync(MinioLocation location);
    Task<MinioLocation> SaveImageAsync(IFormFile file, string bucketName);
}
