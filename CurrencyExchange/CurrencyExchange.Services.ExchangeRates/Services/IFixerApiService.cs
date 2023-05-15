using CurrencyExchange.Services.ExchangeRates.Models;

namespace CurrencyExchange.Services.ExchangeRates.Services
{
    public interface IFixerApiService
    {
        Task<BaseCurrencyRate> GetLatest(string @base, string symbols);
        Task<SymbolsResponse> GetSymbols();
        Task<Dictionary<string, BaseCurrencyRate>> GetLatestForAll(IEnumerable<string> symbols);
    }
}
