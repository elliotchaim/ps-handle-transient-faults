using Newtonsoft.Json;

namespace CarvedRockSoftware.Seeder.CosmosDB
{
    public class ProductItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Ean13 { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
