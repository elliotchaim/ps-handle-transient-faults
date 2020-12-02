// using Microsoft.EntityFrameworkCore.Storage;

namespace CarvedRockSoftware.Seeder.AzureSql
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Ean13 { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
