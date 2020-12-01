using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace CarvedRockSoftware.Seeder.AzureSearch
{
    public class ProductDocument
    {
        [SimpleField(IsKey = true)]
        public string Ean13 { get; set; }

        [SearchableField]
        public string Name { get; set; }

        [SearchableField]
        public string Description { get; set; }
    }
}
