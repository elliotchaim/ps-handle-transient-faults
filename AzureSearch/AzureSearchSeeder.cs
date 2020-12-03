using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Bogus;
using Spectre.Console;

namespace CarvedRockSoftware.Seeder.AzureSearch
{
    public class AzureSearchSeeder : ISeeder
    {
        private const string ServiceEndpointUri = "";
        private const string ApiKey = "";

        private readonly SearchIndexClient _searchIndexClient;
        private readonly IEnumerable<ProductDocument> _seedData;

        public AzureSearchSeeder()
        {
            _searchIndexClient = new SearchIndexClient(
                new Uri(ServiceEndpointUri),
                new AzureKeyCredential(ApiKey));

            var faker = new Faker();
            _seedData = Enumerable.Range(1, 100).Select(i => new ProductDocument
            {
                Ean13 = faker.Commerce.Ean13(),
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription()
            });
        }

        public async Task RunAsync()
        {
            try
            {
                AnsiConsole.MarkupLine("[green]Seeding azure search...[/]");
                var fields = new FieldBuilder().Build(typeof(ProductDocument));
                var index = new SearchIndex(nameof(ProductDocument).ToLower(), fields);
                await _searchIndexClient.CreateOrUpdateIndexAsync(index);

                var searchClient = _searchIndexClient.GetSearchClient(nameof(ProductDocument).ToLower());
                await Task.WhenAll(_seedData.Select(async product => await IndexProduct(searchClient, product)));
            }
            catch (Exception exception)
            {
                AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
            }
            finally
            {
                AnsiConsole.MarkupLine("[green]Seeded azure search.[/]");
            }
        }

        private async Task<Response<IndexDocumentsResult>> IndexProduct(SearchClient searchClient, ProductDocument product)
        {
            var batch = IndexDocumentsBatch.Create(IndexDocumentsAction.Upload(product));
            return await searchClient.IndexDocumentsAsync(batch);
        }
    }
}
