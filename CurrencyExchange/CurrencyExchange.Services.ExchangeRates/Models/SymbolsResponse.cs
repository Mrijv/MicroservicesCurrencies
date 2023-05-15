namespace CurrencyExchange.Services.ExchangeRates.Models
{
    public class SymbolsResponse
    {
        public bool Success { get; set; }
        public Dictionary<string, string> Symbols { get; set; }
    }
}
