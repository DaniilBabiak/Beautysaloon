using HealthChecks.UI.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BeautySaloon.HealthChecksUI.Contorllers;
[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;

    public HealthController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [HttpGet("Identity")]
    public async Task<IActionResult> GetIdentityHealth()
    {
        if (_memoryCache.TryGetValue($"identity_healthcheck_result", out UIHealthReport result))
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpGet("API")]
    public async Task<IActionResult> GetAPIHealth()
    {
        if (_memoryCache.TryGetValue($"api_healthcheck_result", out UIHealthReport result))
        {
            return Ok(result);
        }

        return NotFound();
    }
}
