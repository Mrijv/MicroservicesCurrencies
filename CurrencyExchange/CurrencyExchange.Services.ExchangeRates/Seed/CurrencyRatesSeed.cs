using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Models;
using CurrencyExchange.Services.ExchangeRates.Repositories;
using CurrencyExchange.Services.ExchangeRates.Services;
using System.Linq;

namespace CurrencyExchange.Services.ExchangeRates.Seed
{
    public class CurrencyRatesSeed
    {
        private readonly IFixerApiService fixerService;
        private readonly IAsyncRepository<CurrencyRate> currencyRateRepository;
        private readonly IAsyncRepository<Symbol> symbolRepository;
        private readonly IMapper mapper;

        public CurrencyRatesSeed(IFixerApiService fixerService, IAsyncRepository<CurrencyRate> currencyRateRepository,
            IAsyncRepository<Symbol> symbolRepository, IMapper mapper)
        {
            this.fixerService = fixerService;
            this.currencyRateRepository = currencyRateRepository;
            this.symbolRepository = symbolRepository;
            this.mapper = mapper;
        }

        public async Task SeedAsync()
        {
            var symbolResponse = await fixerService.GetSymbols();
            //change to get default
            IReadOnlyList<CurrencyRate> currencyRates = await currencyRateRepository.ListAllAsync();

            if (currencyRates.Count != 0)
                await AddRatesOfNewSymbol(symbolResponse);
            else
                await InitialSeed(symbolResponse);
        }

        private async Task AddRatesOfNewSymbol(SymbolsResponse symbolResponse)
        {
            var mySymbols = await symbolRepository.ListAllAsync();
            var symbolsFromApi = symbolResponse.Symbols.Keys.ToList();
            var newSymbols = mySymbols.Select(s => s.SymbolTag).Except(symbolsFromApi);

            foreach (var symbol in newSymbols)
            {  //Add symbol as base and its all rates with existing symbols              
                var joinedSymbols = String.Join(',', symbolsFromApi);

                var baseCurrencyRate = await fixerService.GetLatest(symbol, joinedSymbols);

                if (baseCurrencyRate != null)
                {
                    var newCurrencyRates = mapper.Map<List<CurrencyRate>>(baseCurrencyRate);

                    foreach (var currencyRate in newCurrencyRates)
                    {
                        await currencyRateRepository.AddAsync(currencyRate);
                    }
                }

                //Add rate of new symbol for other base symbol
                foreach (var key in symbolResponse.Symbols.Keys)
                {
                    var currencyRate = await fixerService.GetLatest(key, symbol);

                    if (currencyRate != null)
                    {
                        var newCurrencyRates = mapper.Map<CurrencyRate>(baseCurrencyRate);
                        await currencyRateRepository.AddAsync(newCurrencyRates);
                    }
                }
            }
        }

        private async Task InitialSeed(SymbolsResponse symbolResponse)
        {
            if (symbolResponse == null)
                return;

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
