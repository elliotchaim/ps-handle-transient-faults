using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace CarvedRockSoftware.Seeder.AzureSql
{
    public class AzureSqlSeeder : ISeeder
    {
        private const string ConnectionString = "";
        
        private readonly IEnumerable<ProductEntity> _seedData;

        public AzureSqlSeeder()
        {
            var faker = new Faker();
            _seedData = Enumerable.Range(1, 15).Select(i => new ProductEntity
            {
                Ean13 = faker.Commerce.Ean13(),
                Category = faker.Commerce.Categories(1).First(),
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription()
            });
        }

        public async Task RunAsync()
        {
            using (var db = new ApplicationDbContext(ConnectionString))
            {
                await db.Database.MigrateAsync();

                foreach (var product in _seedData)
                {
                    try
                    {
                        await db.AddAsync(product);
                        await db.SaveChangesAsync();

                        AnsiConsole.MarkupLine($"[green]{product.Ean13} seeded.[/]");
                    }
                    catch (Exception exception)
                    {
                        AnsiConsole.MarkupLine($"[red]{product.Ean13} failed.[/]");
                    }
                }
            }
        }
    }
}
