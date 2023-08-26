using BeautySaloon.API.Models;
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
    public async Task<IActionResult> CreateReservationAnonymousAsync(CreateAnonymousReservationRequest createAnonymousReservationRequest)
    {
        var result = await _reservationService.CreateAnonymousReservationAsync(createAnonymousReservationRequest);

        return Ok(result);
    }

    [HttpGet("getAvailable/{serviceId}")]
    public async Task<IActionResult> GetAvailableReservationsForServiceAsync(int serviceId)
    {
        var availableReservations = await _reservationService.GetAvailableReseravtionsByServiceId(serviceId);

        var result = new List<GetAvailableReservationsForServiceResponse>();

        result.AddRange(availableReservations.OrderBy(ar => ar.DateTime).Select(ar => new GetAvailableReservationsForServiceResponse
        {
            AvailableTime = ar.DateTime,
            Master = ar.Master,
            Service = ar.Service
        }));

        return Ok(result);
    }
}
