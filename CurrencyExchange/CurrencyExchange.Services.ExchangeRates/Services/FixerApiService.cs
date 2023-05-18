using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Extensions;
using CurrencyExchange.Services.ExchangeRates.Models;
using CurrencyExchange.Services.ExchangeRates.Repositories;

namespace CurrencyExchange.Services.ExchangeRates.Services
{
    public class FixerApiService : IFixerApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrencyRateRepository currencyRepository;
        private readonly IMapper mapper;
        private readonly ILogger<FixerApiService> logger;
        private const string errorMessage = "Third-party API returned its server error or its response wasn't mapped correctly";

        public FixerApiService(HttpClient httpClient, ICurrencyRateRepository currencyRepository,
            IMapper mapper, ILogger<FixerApiService> logger)
        {
            this.currencyRepository = currencyRepository;
            this.mapper = mapper;
            this.logger = logger;
            _httpClient = httpClient;            
            _httpClient.DefaultRequestHeaders.Add("apikey", "QqTWNjbjznlYwHiIOKOQzuya3I9RjQws");
            _httpClient.Timeout = TimeSpan.FromMinutes(3);
        }

        public async Task<BaseCurrencyRate> GetLatest(string @base, string symbols)
        {
            var apiUri = "/fixer/latest";
            var queryString = $"base={@base}&symbols={Uri.EscapeDataString(symbols)}";
            BaseCurrencyRate result = null;

            try
            {
                var response = await _httpClient.GetAsync($"{apiUri}?{queryString}");
                result = await response.ReadContentAs<BaseCurrencyRate>();
            }
            catch (Exception e)
            {
                logger.LogError(errorMessage, e);
            }

            return result;
        }

        public async Task<Dictionary<string, BaseCurrencyRate>> GetLatestForAll(IEnumerable<string> symbols)
        {
            var symbolsAsParams = String.Join(',', symbols);
            Dictionary<string, BaseCurrencyRate> currenciesRates = new();
            BaseCurrencyRate? baseCurrencyRates = null;
            var counter = 0;

            foreach (var symbol in symbols)
            {
                //I had to limit the number of calls as Free Api key has its own limitations
                if (counter == 3)
                    break;

                var apiUri = "/exchangerates_data/latest";
                var queryString = $"base={symbol}&symbols={Uri.EscapeDataString(symbolsAsParams)}";
                counter++;

                try
                {
                    var response = await _httpClient.GetAsync($"{apiUri}?{queryString}");
                    baseCurrencyRates = await response.ReadContentAs<BaseCurrencyRate>();
                }
                catch (Exception e)
                {
                    logger.LogError(errorMessage, e);
                }

                if (baseCurrencyRates == null)
                {
                    logger.LogDebug("Symbol was omited and haven't been added to the database");
                    break;
                }

                currenciesRates.Add(symbol, baseCurrencyRates);
            }

            return (currenciesRates);
        }

        public async Task<SymbolsResponse> GetSymbols()
        {
            var apiUri = "/exchangerates_data/symbols";            

            try
            {
                var response = await _httpClient.GetAsync($"{apiUri}");
                return await response.ReadContentAs<SymbolsResponse>();
            }
            catch (Exception e)
            {
                logger.LogError(errorMessage, e);
                return null;
            }
        }

        public async Task<decimal> AddRate(string baseCurrency, string currencyTo)
        {
            var apiUri = "/exchangerates_data/latest";
            var queryString = $"base={baseCurrency}&symbols={currencyTo}";

            try
            {
                var response = await _httpClient.GetAsync($"{apiUri}?{queryString}");
                var responseObject = await response.ReadContentAs<BaseCurrencyRate>();

                if (responseObject == null)
                    return decimal.Zero;

                var addedRate = await AddRateToDb(responseObject);

                if(addedRate == null)
                    return decimal.Zero;

                return addedRate.Rate;
            }
            catch (Exception e)
            {
                //log
            }

            return decimal.Zero;
        }

        private async Task<CurrencyRate> AddRateToDb(BaseCurrencyRate baseCurrencyRate)
        {
            var baseCurrency = mapper.Map<CurrencyRate>(baseCurrencyRate);
            return await currencyRepository.AddAsync(baseCurrency);
        }

    }
}
