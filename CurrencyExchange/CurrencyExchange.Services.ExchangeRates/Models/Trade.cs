namespace CurrencyExchange.Services.ExchangeRates.Models
{
    public class Trade
    {
        public decimal Amount { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public Guid UserId { get; set; }
    }
}
