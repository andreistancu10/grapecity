using Newtonsoft.Json;

namespace DigitNow.Adapters.MS.Catalog.Poco
{
    public class DocumentType
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("resolutionPeriod")]
        public int ResolutionPeriod { get; set; }
    }
}
