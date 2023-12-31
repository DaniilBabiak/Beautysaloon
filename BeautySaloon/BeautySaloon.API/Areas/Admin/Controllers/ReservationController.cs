﻿using BeautySaloon.API.Helpers;
using BeautySaloon.API.Models.ReservationModels;
using BeautySaloon.API.Services.Interfaces;
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
    public async Task<IActionResult> GetAllReservationsAsync([FromQuery] int pageNumber = 1,
                                                             [FromQuery] int pageSize = 10,
                                                             [FromQuery] string sortBy = "DateTime")
    {
        var reservations = await _reservationService.GetAllReservationsAsync();

        bool isDescending = sortBy.StartsWith('-');
        if (isDescending)
        {
            sortBy = sortBy[1..]; // Remove the '-' prefix
        }

        Func<ReservationModel, IComparable> sortingKeySelector = sortBy switch
        {
            "Master" => reservation => reservation.MasterName,
            "Service" => reservation => reservation.ServiceName,
            _ => reservation => reservation.DateTime
        };

        var (pageItems, totalPages) = PaginationHelper.PaginateAndSort(
            reservations,
            sortingKeySelector,
            pageNumber,
            pageSize,
            isDescending);

        var response = new
        {
            PageItems = pageItems,
            TotalPages = totalPages
        };

        return Ok(response);
    }

    [HttpGet("getAll/{serviceId}")]
    public async Task<IActionResult> GetAllReservationsForServiceAsync(int serviceId)
    {
        var result = await _reservationService.GetReservationsByServiceId(serviceId);

        return Ok(result);
    }
}
