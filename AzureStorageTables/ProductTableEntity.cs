using Microsoft.Azure.Cosmos.Table;

namespace CarvedRockSoftware.Seeder.AzureStorageTables
{
    public class ProductTableEntity : TableEntity
    {
        public string Ean13 { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
