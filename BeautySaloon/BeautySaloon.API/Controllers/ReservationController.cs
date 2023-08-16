using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Duende.IdentityServer.Extensions;
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

    [HttpGet("getAll")]
    [Authorize("admin")]
    public async Task<IActionResult> GetAllReservationsAsync()
    {
        var result = await _reservationService.GetAllReservationsAsync();
        return Ok(result);
    }

    [HttpGet("getAll/{serviceId}")]
    [Authorize("admin")]
    public async Task<IActionResult> GetAllReservationsForServiceAsync(int serviceId)
    {
        var result = await _reservationService.GetReservationsByServiceId(serviceId);

        return Ok(result);
    }

    [HttpGet("getAvailable/{serviceId}")]
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
    public async Task<IActionResult> CreateReservationAsync(CreateReservationRequest createReservationRequest)
    {
        var customerId = User.Identity.Name;
        var reservation = new Reservation
        {
            ServiceId = createReservationRequest.ServiceId,
            CustomerId = customerId,
            DateTime = createReservationRequest.DateTime
        };

        var result = await _reservationService.CreateReservationAsync(reservation);
        return Ok(result);
    }
}
