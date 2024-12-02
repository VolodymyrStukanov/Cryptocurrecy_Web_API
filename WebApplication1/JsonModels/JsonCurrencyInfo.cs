using Newtonsoft.Json;

namespace WebApplication1.JsonModels
{
    public class JsonCurrencyInfo
    {

        [JsonProperty("asset_id_base")]
        public required string CurrencyAssetId { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("time")]
        public DateTime DateTime { get; set; }
    }
}
