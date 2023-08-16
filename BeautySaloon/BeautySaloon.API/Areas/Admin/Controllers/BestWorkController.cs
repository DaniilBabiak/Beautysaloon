using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Admin.Controllers;

public class BestWorkController : AdminControllerBase
{
    private readonly IBestWorkService _bestWorkService;

    public BestWorkController(IBestWorkService bestWorkService)
    {
        _bestWorkService = bestWorkService;
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
