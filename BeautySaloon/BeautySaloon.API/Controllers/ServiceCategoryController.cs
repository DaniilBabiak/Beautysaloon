using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ServiceCategoryController : ControllerBase
{
    private readonly IServiceCategoryService _category;

    public ServiceCategoryController(IServiceCategoryService category)
    {
        _category = category;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategoriesAsync([FromQuery] int? categoryId)
    {
        if (categoryId.HasValue)
        {
            var result = await _category.GetCategoryByIdAsync(categoryId.Value);
            return Ok(result);
        }
        else
        {
            var result = await _category.GetAllCategoriesAsync();
            return Ok(result);
        }
    }

    // POST api/<ServiceController>
    [HttpPost]
    [Authorize("admin")]
    public async Task<IActionResult> Post([FromBody] ServiceCategory category)
    {
        var test = User;
        var result = await _category.CreateCategoryAsync(category);

        return Ok(result);
    }

    [HttpPut]
    [Authorize("admin")]
    public async Task<IActionResult> Put([FromBody] ServiceCategory category)
    {
        var result = await _category.UpdateCategoryAsync(category);

        return Ok(result);
    }

    // DELETE api/<ServiceController>/5
    [HttpDelete("{id}")]
    [Authorize("admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _category.DeleteCategoryAsync(id);

        return Ok();
    }
}
