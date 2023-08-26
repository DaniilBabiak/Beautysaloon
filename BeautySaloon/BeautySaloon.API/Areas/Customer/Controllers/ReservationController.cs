using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeautySaloon.API.Areas.Customer.Controllers;

public class ReservationController : CustomerControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservationAsync(CreateReservationRequest createReservationRequest)
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var result = await _reservationService.CreateReservationAsync(createReservationRequest, customerId);
        return Ok(result);
    }
}
