namespace CurrencyExchange.Services.ExchangeRates.Services
{
    public interface ITradeService
    {
        Task<decimal> MakeTrade(Models.Trade trade, decimal rate);
        Task<decimal> RetrieveRate(string baseCurrency, string currencyTo);
    }
}
