namespace ProducerApi.Dtos;

public class NewCarDto
{
    public string BrandName { get; init; } = String.Empty;
    public DateTime DateProduced { get; init; }
}