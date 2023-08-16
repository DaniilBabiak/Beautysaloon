using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
}
