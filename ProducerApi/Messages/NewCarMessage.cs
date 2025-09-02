using ProducerApi.Models;

namespace ProducerApi.Messages;

public sealed class NewCarMessage
{
    public required Car Car { get; init; }
    public string FactoryNotes { get; set; } = String.Empty;
}