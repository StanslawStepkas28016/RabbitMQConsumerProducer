using ProducerApi.Messages;

namespace ProducerApi.Services;

public interface IMessageProducer
{
    Task PublishMessage<T>(string queueName, T message, CancellationToken cancellationToken);
}