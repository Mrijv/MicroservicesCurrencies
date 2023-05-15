using CurrencyExchange.Services.ExchangeRates.Models;

namespace CurrencyExchange.Services.ExchangeRates.Services
{
    public interface IFixerApiService
    {
        Task<BaseCurrencyRate> GetLatest(string @base, string symbols);
    }
}
