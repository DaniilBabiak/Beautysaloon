using BeautySaloon.API.Models;
using BeautySaloon.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        var id = User.Identity.Name;
        var customer = await _customerService.GetCustomerAsync(id);

        var result = new GetProfileResponse
        {
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber
        };

        return Ok(result);
    }
}
