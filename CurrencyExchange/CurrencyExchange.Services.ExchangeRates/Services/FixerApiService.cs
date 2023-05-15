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
            _httpClient.DefaultRequestHeaders.Add("apikey", "QqTWNjbjznlYwHiIOKOQzuya3I9RjQws");
            _httpClient.Timeout = TimeSpan.FromMinutes(3);
        }

        public async Task<BaseCurrencyRate> GetLatest(string @base, string symbols)
        {
            var apiUri = "/fixer/latest";
            var queryString = $"base={@base}&symbols={Uri.EscapeDataString(symbols)}";
            var response = await _httpClient.GetAsync($"{apiUri}?{queryString}");
            return await response.ReadContentAs<BaseCurrencyRate>();
        }

        public async Task<Dictionary<string, BaseCurrencyRate>> GetLatestForAll(IEnumerable<string> symbols)
        {
            var symbolsAsParams = String.Join(',', symbols);
            Dictionary<string, BaseCurrencyRate> currenciesRates = new();
            BaseCurrencyRate baseCurrencyRates = null;
            var counter = 0;

            foreach (var symbol in symbols)
            {
                if (counter == 3)
                    break;

                var apiUri = "/exchangerates_data/latest";
                var queryString = $"base={symbol}&symbols={Uri.EscapeDataString(symbolsAsParams)}";
                var response = await _httpClient.GetAsync($"{apiUri}?{queryString}");
                counter++;
                
                try
                {
                    baseCurrencyRates = await response.ReadContentAs<BaseCurrencyRate>();
                }
                catch (Exception e)
                {
                    //log
                    //return null;
                }

                if (baseCurrencyRates == null)
                    break;

                currenciesRates.Add(symbol, baseCurrencyRates);
            }

            return (currenciesRates);
        }

        public async Task<SymbolsResponse> GetSymbols()
        {
            var apiUri = "/exchangerates_data/symbols";
            var response = await _httpClient.GetAsync($"{apiUri}");

            try
            {
                return await response.ReadContentAs<SymbolsResponse>();
            }
            catch (Exception e)
            {
                //log
                return null;
            }
        }

    }
}
