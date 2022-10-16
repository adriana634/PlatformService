using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly RabbitMQOptions configuration;
        private readonly ILogger<MessageBusClient> logger;

        private readonly IConnection? connection = null;
        private readonly IModel? channel = null;

        public MessageBusClient(IOptions<RabbitMQOptions> configuration, ILogger<MessageBusClient> logger)
        {
            this.configuration = configuration.Value;
            this.logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = this.configuration.Host,
                Port = this.configuration.Port
            };

            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;

                this.logger.LogInformation("Connected to Message Bus");
            }
            catch (Exception ex)
            {
                this.logger.LogError("Could not connect to the Message Bus: {ExceptionMessage}", ex.Message);
                throw;
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body
            );
            logger.LogInformation("Message sent: {Message}", message);
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (connection!.IsOpen)
            {
                logger.LogInformation("RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                logger.LogInformation("RabbitMQ Connection is closed, not sending");
            }
        }

        private void RabbitMQ_ConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            logger.LogInformation("Message Bus connection Shutdown");
        }

        public void Dispose()
        {
            if (connection is not null && channel is not null)
            {
                if (channel.IsOpen)
                {
                    channel.Close();
                    connection.Close();

                    connection.ConnectionShutdown -= RabbitMQ_ConnectionShutDown;

                    channel.Dispose();
                    connection.Dispose();

                    logger.LogInformation("MessageBus disposed");
                }
            }
        }
    }
}