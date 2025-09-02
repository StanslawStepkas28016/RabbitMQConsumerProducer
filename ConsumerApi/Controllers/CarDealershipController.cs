using Microsoft.AspNetCore.Mvc;

namespace ConsumerApi.Controllers;

[ApiController]
[Route("api/consumer")]
public class CarDealershipController : ControllerBase
{
    private readonly ILogger<CarDealershipController> _logger;

    public CarDealershipController(ILogger<CarDealershipController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}