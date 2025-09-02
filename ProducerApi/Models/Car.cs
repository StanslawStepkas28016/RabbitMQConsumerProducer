namespace ProducerApi.Models;

public sealed class Car
{
    public Guid IdCar { get; init; }
    public string BrandName { get; init; } = String.Empty;
    public DateTime DateProduced { get; init; }
}