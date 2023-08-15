using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeautySaloon.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _service;

    public ServiceController(IServiceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetServicesAsync([FromQuery] int? categoryId)
    {
        if (categoryId.HasValue)
        {
            var result = await _service.GetServicesByCategoryIdAsync(categoryId.Value);
            return Ok(result);
        }
        else
        {
            var result = await _service.GetAllServicesAsync();
            return Ok(result);
        }
    }

    // POST api/<ServiceController>
    [HttpPost]
    [Authorize("admin")]
    public async Task<IActionResult> Post([FromBody] Service service)
    {
        var result = await _service.CreateServiceAsync(service);

        return Ok(result);
    }

    // PUT api/<ServiceController>/5
    [HttpPut("{id}")]
    [Authorize("admin")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ServiceController>/5
    [HttpDelete("{id}")]
    [Authorize("admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteServiceAsync(id);

        return Ok();
    }
}
