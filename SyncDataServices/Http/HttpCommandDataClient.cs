using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<HttpCommandDataClient> logger;

        public HttpCommandDataClient(HttpClient httpClient,
                                     IConfiguration configuration,
                                     ILogger<HttpCommandDataClient> logger)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(configuration["CommandService"], httpContent);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Sync POST to CommandService was OK!");
            }
            else
            {
                logger.LogInformation("Sync POST to CommandService was NOT OK!");
            }
        }
    }
}