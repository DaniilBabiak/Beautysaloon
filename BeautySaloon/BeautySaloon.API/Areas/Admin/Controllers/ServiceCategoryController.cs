using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Admin.Controllers;

public class ServiceCategoryController : AdminControllerBase
{
    private readonly IServiceCategoryService _category;

    public ServiceCategoryController(IServiceCategoryService category)
    {
        _category = category;
    }

    [HttpPost]
    [Authorize("admin")]
    public async Task<IActionResult> CreateCategoryAsync([FromBody] ServiceCategory category)
    {
        var test = User;
        var result = await _category.CreateCategoryAsync(category);

        return Ok(result);
    }

    [HttpPut]
    [Authorize("admin")]
    public async Task<IActionResult> UpdateCategoryAsync([FromBody] ServiceCategory category)
    {
        var result = await _category.UpdateCategoryAsync(category);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize("admin")]
    public async Task<IActionResult> DeleteCategoryAsync(int id)
    {
        await _category.DeleteCategoryAsync(id);

        return Ok();
    }
}
