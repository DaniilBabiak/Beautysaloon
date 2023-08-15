using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BestWorkController : ControllerBase
{
    private readonly IBestWorkService _bestWorkService;

    public BestWorkController(IBestWorkService bestWorkService)
    {
        _bestWorkService = bestWorkService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBestWorksAsync()
    {
        var result = await _bestWorkService.GetAllBestWorksAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBestWorkAsync([FromBody] BestWork bestWork)
    {
        var result = await _bestWorkService.CreateBestWorkAsync(bestWork);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBestWorkAsync(int id)
    {
        await _bestWorkService.DeleteBestWorkAsync(id);
        return Ok();
    }
}
