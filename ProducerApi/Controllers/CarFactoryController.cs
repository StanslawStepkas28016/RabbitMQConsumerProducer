using Microsoft.AspNetCore.Mvc;
using ProducerApi.Dtos;
using ProducerApi.Messages;
using ProducerApi.Models;
using ProducerApi.Services;
using RabbitMQ.Client;

namespace ProducerApi.Controllers;

[ApiController]
[Route("api/car-factory")]
public class CarFactoryController : ControllerBase
{
    private readonly ILogger<CarFactoryController> _logger;
    private readonly IMessageProducer _messageProducer;

    public CarFactoryController(ILogger<CarFactoryController> logger, IMessageProducer messageProducer)
    {
        _logger = logger;
        _messageProducer = messageProducer;
    }

    [HttpPost("release")]
    public async Task<IActionResult> ReleaseACar([FromBody] NewCarDto newCarDto, CancellationToken cancellationToken)
    {
        // Fake some business logic.
        Car car = new Car
        {
            IdCar = Guid.NewGuid(),
            BrandName = newCarDto.BrandName,
            DateProduced = newCarDto.DateProduced
        };

        NewCarMessage message = new NewCarMessage
        {
            Car = car,
            FactoryNotes = "Some notes."
        };

        // Send the message to the queue.
        await _messageProducer.PublishMessage<NewCarMessage>("release-car", message, cancellationToken);

        return Ok(message);
    }
}