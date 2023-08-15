using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{serviceId}")]
    public async Task<IActionResult> GetAvailableReservationsForServiceAsync(int serviceId)
    {
        var availableReservations = await _reservationService.GetAvailableReseravtionsByServiceId(serviceId);

        var result = new GetAvailableReservationsForServiceResponse
        {
            Service = availableReservations.First()?.Service,
            AvailableReservations = availableReservations.Select(r => r.DateTime)
        };

        return Ok(result);
    }

    [HttpPost]
    [Authorize("api.edit")]
    public async Task<IActionResult> CreateReservationAsync(Reservation reservation)
    {
        var result = await _reservationService.CreateReservationAsync(reservation);
        return Ok(result);
    }
}
