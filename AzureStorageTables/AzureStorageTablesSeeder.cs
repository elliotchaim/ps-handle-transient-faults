using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Azure.Cosmos.Table;
using Spectre.Console;

namespace CarvedRockSoftware.Seeder.AzureStorageTables
{
    public class AzureStorageTablesSeeder : ISeeder
    {
        private const string ConnectionString = "";

        private readonly CloudTableClient _cloudTableClient;
        private readonly IEnumerable<ProductTableEntity> _seedData;

        public AzureStorageTablesSeeder()
        {
            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            _cloudTableClient = storageAccount.CreateCloudTableClient();

            _cloudTableClient.DefaultRequestOptions.RetryPolicy = new NoRetry();

            var faker = new Faker();
            _seedData = Enumerable.Range(1, 10).Select(i =>
            {
                var ean13 = faker.Commerce.Ean13();
                var category = faker.Commerce.Categories(1).First();
                var name = faker.Commerce.ProductName();
                var description = faker.Commerce.ProductDescription();

                return new ProductTableEntity
                {
                    PartitionKey = category,
                    RowKey = ean13,
                    Ean13 = ean13,
                    Name = name,
                    Category = category,
                    Description = description
                };
            });
        }

        public async Task RunAsync()
        {
            var faultyOperationContext = new OperationContext();
            faultyOperationContext.ResponseReceived += (sender, args) =>
             {
                 if (new Random().Next(3) == 0)
                 {
                     args.Response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                 }
             };

            var table = _cloudTableClient.GetTableReference("products");
            await table.CreateIfNotExistsAsync();

            await Task.WhenAll(_seedData.Select(async (product) =>
            {
                try
                {
                    await table.ExecuteAsync(TableOperation.InsertOrReplace(product),
                        new TableRequestOptions(), faultyOperationContext);
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
