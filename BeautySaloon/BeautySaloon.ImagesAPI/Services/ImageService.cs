using BeautySaloon.ImagesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Minio;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.IO.Pipelines;
using System.Net.Mime;

namespace BeautySaloon.ImagesAPI.Services;

public class ImageService : IImageService
{
    private readonly MinioClient _minio;

    public ImageService(MinioClient minio)
    {
        _minio = minio;
    }

    public async Task<MinioLocation> SaveImageAsync(IFormFile file, string bucketName)
    {
        var fileName = file.FileName;
        var resizedImageTask = ResizeImage(file);

        var ensureBucketExistsTask = EnsureBucketExists(_minio, bucketName);

        await Task.WhenAll(ensureBucketExistsTask, resizedImageTask);

        var resizedImage = resizedImageTask.Result;

        using (var imageStream = new MemoryStream())
        {
            var imageFormat = resizedImage.Metadata.DecodedImageFormat;
            var formatName = imageFormat.Name;

            resizedImage.Save(imageStream, imageFormat); // Замените на нужный формат

            imageStream.Seek(0, SeekOrigin.Begin);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithObjectSize(imageStream.Length)
                .WithStreamData(imageStream)
                .WithContentType(GetContentTypeFromImageFormat(formatName)); // Замените на соответствующий MIME тип
            var putResponse = await _minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
            var result = new MinioLocation
            {
                FileName = putResponse.ObjectName,
                BucketName = bucketName
            };

            return result;
        }
    }

    public byte[] GetImageAsync(MinioLocation location)
    {
        byte[] result = default;

        var getObjectArgs = new GetObjectArgs().WithBucket(location.BucketName)
                                               .WithObject(location.FileName)
                                               .WithCallbackStream(stream =>
                                               {
                                                   using MemoryStream ms = new MemoryStream();
                                                   stream.CopyTo(ms);
                                                   result = ms.ToArray();
                                               });

        _minio.GetObjectAsync(getObjectArgs).Wait();
        // Возвращаем файл как результат
        return result;
    }

    private async Task EnsureBucketExists(MinioClient minio, string bucketName)
    {
        var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
        bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
        if (!found)
        {
            var mbArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
        }
    }

    private async Task<Image> ResizeImage(IFormFile file)
    {
        using (var image = await Image.LoadAsync(file.OpenReadStream()))
        {
            var resizedImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
            {
                Size = new Size(1000, 667),
                Mode = ResizeMode.Max
            }));

            return resizedImage;
        }
    }

    private string GetContentTypeFromImageFormat(string formatName)
    {
        switch (formatName)
        {
            case "jpeg":
                return "image/jpeg";
            case "png":
                return "image/png";
            default:
                return "image/jpeg";
        }
    }
}
