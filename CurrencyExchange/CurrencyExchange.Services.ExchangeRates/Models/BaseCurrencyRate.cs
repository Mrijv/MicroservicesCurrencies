using System.Text.Json.Serialization;

namespace CurrencyExchange.Services.ExchangeRates.Models
{
    public class BaseCurrencyRate
    {
        [JsonPropertyName("base")]
        public string BaseCurrency { get; set; }
        [JsonPropertyName("date")]
        public DateTime RetrievalDate { get; set; }
        [JsonPropertyName("rates")]
        public Rates Rates { get; set; }
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("timestamp")]
        public int TimeStamp { get; set; }
    }
}
