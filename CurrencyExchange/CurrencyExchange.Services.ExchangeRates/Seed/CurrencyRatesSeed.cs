using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Models;
using CurrencyExchange.Services.ExchangeRates.Repositories;
using CurrencyExchange.Services.ExchangeRates.Services;

namespace CurrencyExchange.Services.ExchangeRates.Seed
{
    public class CurrencyRatesSeed
    {
        private readonly IFixerApiService fixerService;
        private readonly ICurrencyRateRepository currencyRateRepository;
        private readonly IAsyncRepository<Symbol> symbolRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CurrencyRatesSeed> logger;
        private const string criticalMessage = "Third-party API servers are down or check your license. Restart app to seed/update database with Symbols.";

        public CurrencyRatesSeed(IFixerApiService fixerService, ICurrencyRateRepository currencyRateRepository,
            IAsyncRepository<Symbol> symbolRepository, IMapper mapper, ILogger<CurrencyRatesSeed> logger)
        {
            this.fixerService = fixerService;
            this.currencyRateRepository = currencyRateRepository;
            this.symbolRepository = symbolRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task SeedAsync()
        {
            var symbolResponse = await fixerService.GetSymbols();

            if (symbolResponse == null || symbolResponse.Success.Equals(false))
            {
                logger.LogCritical(criticalMessage);
                return;
            }

            var currencyRate = await currencyRateRepository.ReturnAny();

            if (currencyRate != default || currencyRate != null)
                await UpdateSymbols(symbolResponse);
            else
            {
                try
                {
                    await InitialSeed(symbolResponse);
                }
                catch (Exception e)
                {
                    logger.LogCritical(criticalMessage, e);
                }
            }
               
        }

        private async Task UpdateSymbols(SymbolsResponse symbolResponse)
        {
            var mySymbols = await symbolRepository.ListAllAsync();
            var symbolsFromApi = symbolResponse.Symbols.Keys.ToList();
            var newSymbols = mySymbols.Select(s => s.SymbolTag).Except(symbolsFromApi);

            foreach (var symbol in newSymbols)
            {
                var joinedSymbols = String.Join(',', symbolsFromApi);
                var baseCurrencyRate = await fixerService.GetLatest(symbol, joinedSymbols);

                try
                {
                    await AddBaseRates(baseCurrencyRate);
                    await UpdateBaseRatesWithNewCurrencyToSymbol(symbolsFromApi, symbol, baseCurrencyRate);
                }
                catch (Exception e)
                {
                    logger.LogError("Sth went wrong during updating symbols in the database", e);
                }                
            }
        }

        private async Task AddBaseRates(BaseCurrencyRate baseCurrencyRate)
        {
            if (baseCurrencyRate != null)
            {
                var newCurrencyRates = mapper.Map<List<CurrencyRate>>(baseCurrencyRate);

                foreach (var currencyRate in newCurrencyRates)
                {
                    await currencyRateRepository.AddAsync(currencyRate);
                }
            }
            else
            {
                logger.LogWarning("Coudn't get lates currency exchange rate - Third-party Api has issue.");
            }
        }

        private async Task UpdateBaseRatesWithNewCurrencyToSymbol(List<string> symbolsFromApi, string symbol, BaseCurrencyRate baseCurrencyRate)
        {
            foreach (var key in symbolsFromApi)
            {
                var currencyRate = await fixerService.GetLatest(key, symbol);

                if (currencyRate != null)
                {
                    var newCurrencyRates = mapper.Map<CurrencyRate>(baseCurrencyRate);
                    await currencyRateRepository.AddAsync(newCurrencyRates);
                }
                else
                {
                    logger.LogDebug("Symbol was omited and its currency exchange rate haven't been added to the database");
                }
            }
        }

        private async Task InitialSeed(SymbolsResponse symbolResponse)
        {
            var allRates = await fixerService.GetLatestForAll(symbolResponse.Symbols.Keys);

            foreach (var item in allRates.Values)
            {
                var newCurrencyRates = mapper.Map<List<CurrencyRate>>(item);

                foreach (var currencyRate in newCurrencyRates)
                {
                    await currencyRateRepository.AddAsync(currencyRate);
                }
            }
        }
    }
}
