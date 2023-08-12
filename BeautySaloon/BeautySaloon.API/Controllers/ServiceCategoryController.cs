using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize("admin")]
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
    public async Task<IActionResult> Post([FromBody] ServiceCategory category)
    {
        var result = await _category.CreateCategoryAsync(category);

        return Ok(result);
    }

    // PUT api/<ServiceController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ServiceController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _category.DeleteCategoryAsync(id);

        return Ok();
    }
}
