using BeautySaloon.API.Services.Interfaces;
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
}
