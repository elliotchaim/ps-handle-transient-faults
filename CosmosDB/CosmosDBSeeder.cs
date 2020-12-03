using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Azure.Cosmos;
using Spectre.Console;

namespace CarvedRockSoftware.Seeder.CosmosDB
{
    public class CosmosDBSeeder : ISeeder
    {
        private const string ConnectionString = "";

        private readonly CosmosClient _cosmosClient;
        private readonly IEnumerable<ProductItem> _seedData;

        public CosmosDBSeeder()
        {
            _cosmosClient = new CosmosClient(ConnectionString);

            var faker = new Faker();
            _seedData = Enumerable.Range(1, 100).Select(i => new ProductItem
            {
                Ean13 = faker.Commerce.Ean13(),
                Category = faker.Commerce.Categories(1).First(),
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription()
            });
        }

        public async Task RunAsync()
        {
            await _cosmosClient.CreateDatabaseIfNotExistsAsync("production");
            var database = _cosmosClient.GetDatabase("production");
            await database.CreateContainerIfNotExistsAsync("products", "/Category");
            var container = database.GetContainer("products");

            await Task.WhenAll(_seedData.Select(async product => 
            {
                try
                {
                    await container.CreateItemAsync<ProductItem>(product, new PartitionKey(product.Category));
                    AnsiConsole.MarkupLine($"[green]{product.Ean13} seeded.[/]");
                }
                catch (Exception exception)
                {
                    AnsiConsole.MarkupLine($"[red]{product.Ean13} failed.[/]");
                }
            }));
        }
    }
}
