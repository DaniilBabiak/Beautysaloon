using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.API.Areas.Customer.Controllers;

[Area("Customer")]
[Route("api/[area]/[controller]")]
[ApiController]
[Authorize("customer")]
public abstract class CustomerControllerBase : ControllerBase
{
}