using CurrencyExchange.Services.ExchangeRates.Entities;

namespace CurrencyExchange.Services.ExchangeRates.Repositories
{
    public interface ICurrencyRateRepository : IAsyncRepository<CurrencyRate>
    {
        Task<decimal> RetrieveRate(string currencyFrom, string currencyTo);
        Task UpdateRateAsync(CurrencyRate currencyRate);
        Task<CurrencyRate> ReturnAny();
    }
}
