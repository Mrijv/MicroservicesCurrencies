using CurrencyExchange.Services.ExchangeRates.Extensions;
using CurrencyExchange.Services.ExchangeRates.Models;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.Services.ExchangeRates.Services
{
    public class FixerApiService : IFixerApiService
    {
        private readonly HttpClient _httpClient;

        public FixerApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("apikey", "qGgIyk85BYPiOFigeyHQrecmbKXXZxkX");
        }

        public async Task<BaseCurrencyRate> GetLatest(string @base, string symbols)
        {
            var apiUri = "/fixer/latest";
            var queryString = $"base={@base}&symbols={Uri.EscapeDataString(symbols)}";
            var response = await _httpClient.GetAsync($"{apiUri}?{queryString}");
            return await response.ReadContentAs<BaseCurrencyRate>();
        }

    }
}
