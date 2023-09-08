using AutoMapper;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.BestWorkModels;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Admin.Controllers;

public class BestWorkController : AdminControllerBase
{
    private readonly IBestWorkService _bestWorkService;
    private readonly IMapper _mapper;
    public BestWorkController(IBestWorkService bestWorkService, IMapper mapper)
    {
        _bestWorkService = bestWorkService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBestWorkAsync([FromBody] BestWorkModel bestWorkModel)
    {
        var result = await _bestWorkService.CreateBestWorkAsync(bestWorkModel);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBestWorkAsync(int id)
    {
        await _bestWorkService.DeleteBestWorkAsync(id);
        return Ok();
    }
}
