using Newtonsoft.Json;

namespace DigitNow.Adapters.MS.Catalog.Poco
{
    public class Department
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("orderNumber")]
        public long OrderNumber { get; set; }
    }
}
