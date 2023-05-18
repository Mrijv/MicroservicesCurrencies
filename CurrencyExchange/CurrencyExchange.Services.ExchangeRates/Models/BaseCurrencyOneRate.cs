using System.Text.Json.Serialization;

namespace CurrencyExchange.Services.ExchangeRates.Models
{
    public class BaseCurrencyOneRate
    {
        public string BaseCurrency { get; set; }
        public DateTime RetrievalDate { get; set; }
        public decimal Rate { get; set; }
        public bool Success { get; set; }
        public int TimeStamp { get; set; }
    }
}
