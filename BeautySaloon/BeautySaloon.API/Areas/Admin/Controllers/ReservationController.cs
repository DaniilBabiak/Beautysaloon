using BeautySaloon.API.Entities.BeautySaloonContextEntities;
using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Admin.Controllers;
public class ReservationController : AdminControllerBase
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
}
