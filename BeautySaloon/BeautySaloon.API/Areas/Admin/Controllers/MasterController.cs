using BeautySaloon.API.Models.MasterModels;
using BeautySaloon.API.Models.ScheduleModels;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Admin.Controllers;
public class MasterController : AdminControllerBase
{
    private readonly IMasterService _masterService;

    public MasterController(IMasterService masterService)
    {
        _masterService = masterService;
    }

    [HttpGet("schedule")]
    public async Task<IActionResult> GetSchedule([FromQuery] int? masterId, [FromQuery] int? scheduleId)
    {
        if (masterId.HasValue && scheduleId.HasValue)
        {
            return BadRequest();
        }

        if (masterId.HasValue)
        {
            var result = await _masterService.GetScheduleByMasterIdAsync(masterId.Value);
            return Ok(result);
        }

        if (scheduleId.HasValue)
        {
            var result = await _masterService.GetScheduleAsync(scheduleId.Value);
            return Ok(result);
        }

        return BadRequest();
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> CreateSheduleAsync(ScheduleModel createScheduleModel)
    {
        var result = await _masterService.CreateScheduleAsync(createScheduleModel);

        return Ok(result);
    }

    [HttpPut("schedule")]
    public async Task<IActionResult> UpdateScheduleAsync(ScheduleModel scheduleModel)
    {
        var result = await _masterService.UpdateScheduleAsync(scheduleModel);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMasterAsync(MasterDetailedModel createMasterModel)
    {
        var result = await _masterService.CreateMasterAsync(createMasterModel);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMasterAsync(MasterDetailedModel updateMasterModel)
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
