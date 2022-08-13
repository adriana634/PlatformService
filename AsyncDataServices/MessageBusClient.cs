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

            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = this.configuration.Host,
                Port = this.configuration.Port
            };

            this.connection = null!;
            this.channel = null!;

            try
            {
                this.connection = factory.CreateConnection();
                this.channel = this.connection.CreateModel();

                this.channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                this.connection.ConnectionShutdown += MessageBusClient.RabbitMQ_ConnectionShutDown;

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
            if (this.connection != null && this.channel != null)
            {
                if (this.channel.IsOpen)
                {
                    this.channel.Close();
                    this.connection.Close();

                    this.connection.ConnectionShutdown -= MessageBusClient.RabbitMQ_ConnectionShutDown;

                    this.channel.Dispose();
                    this.connection.Dispose();

                    Console.WriteLine("MessageBus disposed");
                }
            }
        }

        private void SendMessage(string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);

            this.channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body
            );
            Console.WriteLine($"--> Message sent: {message}");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            string message = JsonSerializer.Serialize(platformPublishedDto);

            if (this.connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                this.SendMessage(message);
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