using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Storage.Queues;
using Bogus;
using Newtonsoft.Json;
using Spectre.Console;

namespace CarvedRockSoftware.Seeder.AzureStorageQueues
{
    public class AzureStorageQueuesSeeder : ISeeder
    {
        private const string ConnectionString = "";

        private readonly QueueClient _queueClient;
        private readonly IEnumerable<(int Id, string Name, string Description)> _seedData;

        public AzureStorageQueuesSeeder()
        {
            var queueClientOptions = new QueueClientOptions();
            var policy = new FakeTransientErrorsHttpPipelinePolicy();
            queueClientOptions.AddPolicy(policy, HttpPipelinePosition.PerRetry);
            
            queueClientOptions.Retry.MaxRetries = 0;

            _queueClient = new QueueClient(ConnectionString, "products", queueClientOptions);

            var faker = new Faker();
            _seedData = Enumerable.Range(1, 10).Select(i =>
            {
                var id = i;
                var name = faker.Commerce.ProductName();
                var description = faker.Commerce.ProductDescription();
                return (id, name, description);
            });
        }

        public async Task RunAsync()
        {
            await _queueClient.CreateIfNotExistsAsync();

            await Task.WhenAll(_seedData.Select(async (product) =>
            {
                try
                {
                    var json = JsonConvert.SerializeObject(product);
                    var messageText = JsonConvert.SerializeObject(json);
                    var base64messageString = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageText));
                    await _queueClient.SendMessageAsync(base64messageString);
                    AnsiConsole.MarkupLine($"[green]{product.Name} seeded.[/]");
                }
                catch (Exception exception)
                {
                    AnsiConsole.MarkupLine($"[red]{product.Name} failed.[/]");
                }
            }));
        }
    }
}
