using BeautySaloon.ImagesAPI.Models;
using BeautySaloon.ImagesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BeautySaloon.ImagesAPI.Controllers;
[ApiController]
[Route("/api/[controller]")]
public class ImageController : ControllerBase
{    
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost()]
    public async Task<IActionResult> UploadFile([Required] IFormFile file, [Required, FromQuery] string bucketName)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var validExtensions = new string[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName);
        if (!validExtensions.Contains(fileExtension.ToLower()))
        {
            var supportedFormats = string.Join(", ", validExtensions);
            return BadRequest($"Invalid file format. Supported formats: {supportedFormats}");
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = await _imageService.SaveImageAsync(file, bucketName);
        var elapsedTime = stopwatch.Elapsed;
        return Ok(new { result, elapsedTime });
    }

    [HttpGet]
    public IActionResult GetFile([Required, FromQuery] MinioLocation location)
    {
        var result = _imageService.GetImageAsync(location);
        var contentType = "image/jpeg";

        return File(result, contentType);
    }
}
