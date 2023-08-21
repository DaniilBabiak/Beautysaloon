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

    [HttpGet("schedule/{masterId}")]
    public async Task<IActionResult> GetScheduleAsync(int masterId)
    {
        var result = await _masterService.GetScheduleAsync(masterId);

        return Ok(result);
    }

    [HttpPost("schedule/{masterId}")]
    public async Task<IActionResult> CreateSheduleAsync(int masterId, Schedule schedule)
    {
        var result = await _masterService.CreateScheduleAsync(masterId, schedule);

        return Ok(result);
    }

    [HttpPut("schedule")]
    public async Task<IActionResult> UpdateScheduleAsync(Schedule schedule)
    {
        var result = await _masterService.UpdateScheduleAsync(schedule);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMasterAsync(CreateMasterModel createMasterModel)
    {
        var result = await _masterService.CreateMasterAsync(createMasterModel);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMasterAsync(UpdateMasterModel updateMasterModel)
    {
        var result = await _masterService.UpdateMasterAsync(updateMasterModel);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMasterAsync(int id)
    {
        await _masterService.DeleteMasterAsync(id);
        return Ok();
    }
}
