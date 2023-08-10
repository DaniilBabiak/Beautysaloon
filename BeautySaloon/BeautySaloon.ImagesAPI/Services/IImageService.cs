using BeautySaloon.ImagesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.ImagesAPI.Services;

public interface IImageService
{
    public byte[] GetImageAsync(MinioLocation location);
    Task<MinioLocation> SaveImageAsync(IFormFile file, string bucketName);
}
