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
        private readonly IConnection connection;
        private readonly IModel channel;

        public MessageBusClient(IOptions<RabbitMQOptions> configuration)
        {
            this.configuration = configuration.Value;

            var factory = new ConnectionFactory()
            {
                HostName = this.configuration.Host,
                Port = this.configuration.Port
            };

            connection = null!;
            channel = null!;

            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                connection.ConnectionShutdown += MessageBusClient.RabbitMQ_ConnectionShutDown;

                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            if (connection != null && channel != null)
            {
                if (channel.IsOpen)
                {
                    channel.Close();
                    connection.Close();

                    connection.ConnectionShutdown -= MessageBusClient.RabbitMQ_ConnectionShutDown;

                    channel.Dispose();
                    connection.Dispose();

                    Console.WriteLine("MessageBus disposed");
                }
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
            Console.WriteLine($"--> Message sent: {message}");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection is closed, not sending");
            }
        }

        private static void RabbitMQ_ConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}