using BeautySaloon.API.Models.ReservationModels;
using BeautySaloon.API.Services;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservationAnonymousAsync(ReservationModel reservationModel)
    {
        var result = await _reservationService.CreateReservationAsync(reservationModel);

        return Ok(result);
    }

    [HttpGet("getAvailable")]
    public async Task<IActionResult> GetAvailableReservationsAsync([FromQuery]int serviceId, [FromQuery] int masterId)
    {
        var result = await _reservationService.GetAvailableReseravtionsAsync(serviceId, masterId);

        return Ok(result);
    }
}
