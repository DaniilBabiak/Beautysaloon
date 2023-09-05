using BeautySaloon.API.Areas.Admin.Controllers;
using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models.ServiceModels;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeautySaloon.API.Areas.Admin;

public class ServiceController : AdminControllerBase
{
    private readonly IServiceService _service;

    public ServiceController(IServiceService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateServiceAsync([FromBody] ServiceDetailedModel serviceModel)
    {
        var result = await _service.CreateServiceAsync(serviceModel);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateServiceAsync([FromBody] ServiceDetailedModel serviceModel)
    {
        var result = await _service.UpdateServiceAsync(serviceModel);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceAsync(int id)
    {
        await _service.DeleteServiceAsync(id);

        return Ok();
    }
}
