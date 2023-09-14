using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeautySaloon.API.Areas.Customer.Controllers;
public class ProfileController : CustomerControllerBase
{
    private readonly ICustomerService _customerService;

    public ProfileController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfileAsync()
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var customer = await _customerService.GetCustomerAsync(id);

        var result = new GetProfileResponse
        {
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber
        };

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfileAsync(UpdateProfileRequest updateProfileRequest)
    {
        var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var customer = new Entities.BeautySaloonContextEntities.Customer
        {
            Id = id,
            Name = updateProfileRequest.Name,
            PhoneNumber = updateProfileRequest.PhoneNumber
        };

        var result = await _customerService.UpdateCustomerAsync(customer);

        return Ok(result);
    }
}
