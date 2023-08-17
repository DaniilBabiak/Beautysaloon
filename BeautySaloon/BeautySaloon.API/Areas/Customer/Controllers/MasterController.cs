using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Customer.Controllers;
public class MasterController : CustomerControllerBase
{
    private readonly IMasterService _masterService;

    public MasterController(IMasterService masterService)
    {
        _masterService = masterService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMastersAsync()
    {
        var result = await _masterService.GetAllMastersAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMastersAsync(int id)
    {
        var result = await _masterService.GetMasterAsync(id);
        return Ok(result);
    }
}
