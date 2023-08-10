using Microsoft.AspNetCore.Mvc;
using Minio;

namespace BeautySaloon.API.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastController()
    {
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var endpoint = "your-minio-endpoint";
        var accessKey = "your-access-key";
        var secretKey = "your-secret-key";

        var minio = new MinioClient();
        minio.WithEndpoint(endpoint);
        return Ok(Summaries);
    }
}
