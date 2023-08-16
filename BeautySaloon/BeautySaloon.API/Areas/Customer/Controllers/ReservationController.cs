using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Customer.Controllers;

public class ReservationController : CustomerControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
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
