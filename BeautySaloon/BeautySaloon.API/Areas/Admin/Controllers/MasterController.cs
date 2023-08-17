using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Admin.Controllers;
public class MasterController : AdminControllerBase
{
    private readonly IMasterService _masterService;

    public MasterController(IMasterService masterService)
    {
        _masterService = masterService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMasterAsync(Master master)
    {
        var result = await _masterService.CreateMasterAsync(master);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMasterAsync(Master master)
    {
        var result = await _masterService.UpdateMasterAsync(master);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMasterAsync(int id)
    {
        await _masterService.DeleteMasterAsync(id);
        return Ok();
    }
}
