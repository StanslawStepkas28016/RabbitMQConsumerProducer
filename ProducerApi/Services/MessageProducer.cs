using System.Text;
using System.Text.Json;
using ProducerApi.Messages;
using RabbitMQ.Client;

namespace ProducerApi.Services;

public class MessageProducer : IMessageProducer
{
    public async Task PublishMessage<T>(string queueName, T message, CancellationToken cancellationToken)
    {
        // Ensure correct input.
        if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentException($"{nameof(queueName)}");

        // Create a "setup" object from which you can create a connection.
        ConnectionFactory connectionFactory = new ConnectionFactory
        {
            // This would normally reside in the appsettings.json.
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/"
        };

        // Create a connection.
        IConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        // Create a channel.
        await using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        // Declare an actual queue:
        // - Durable (after the message broker is turned off the messages are persisted).
        // - Exclusive (they may be only accessed by the current connection and are deleted when that connection closes).
        await channel.QueueDeclareAsync
            (queueName, durable: false, exclusive: false, cancellationToken: cancellationToken);

        // Serialize the message to JSON. Then convert it to bytes in order to be able to send it.
        var jsonString = JsonSerializer.Serialize(message);
        var bytes = Encoding.UTF8.GetBytes(jsonString);

        // Publish the message, leave the exchange parameter as empty as we do not care about sending to multiple different consumers.
        await channel.BasicPublishAsync("", queueName, body: bytes, cancellationToken: cancellationToken);
    }
}