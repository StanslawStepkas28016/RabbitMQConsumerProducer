using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerApi.Services;

public class MessageConsumerBackground : BackgroundService
{
    private readonly ILogger<MessageConsumerBackground> _logger;

    public MessageConsumerBackground(ILogger<MessageConsumerBackground> logger)
    {
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
        IConnection connection = await connectionFactory.CreateConnectionAsync(stoppingToken);

        // Create a channel.
        await using IChannel channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        // Declare a queue, the "release-car" would normally reside in the appsettings.json or elsewhere.
        await channel.QueueDeclareAsync
            ("release-car", durable: false, exclusive: false, cancellationToken: stoppingToken);

        // Create a consumer using the appropriate channel.
        AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

        // Subscribe to the Received event and assign an appropriate handler.
        consumer.ReceivedAsync += ReleaseCarHandler;

        // Ensure that you start consuming messages from the event.
        await channel.BasicConsumeAsync
            ("release-car", autoAck: true, consumer: consumer, cancellationToken: stoppingToken);

        // Do not "quit" this method until a cancellation is requested.
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ReleaseCarHandler(object sender, BasicDeliverEventArgs args)
    {
        // Convert the message back to readable UTF-8 string from bytes.
        string body = Encoding.UTF8.GetString(args.Body.ToArray());

        // Log the message.
        _logger.LogCritical("A message containing {body} was received, saving info to the database...", body);

        // Perform some business logic, saving to the db or something else...
    }
}