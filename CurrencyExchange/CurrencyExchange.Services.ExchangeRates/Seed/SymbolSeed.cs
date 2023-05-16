using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Repositories;
using CurrencyExchange.Services.ExchangeRates.Services;

namespace CurrencyExchange.Services.ExchangeRates.Seed
{
    public class SymbolSeed
    {
        public static async Task SeedAsync(IFixerApiService fixerService, IAsyncRepository<Symbol> symbolRepository, IMapper mapper)
        {
            var mySymbols = await symbolRepository.ListAllAsync();
            var symbolResponse = await fixerService.GetSymbols();

            if (mySymbols.Count == symbolResponse.Symbols.Count)
                return;

            if(mySymbols.Count == 0)
            {
                var symbols = mapper.Map<List<Symbol>>(symbolResponse);

                foreach (var symbol in symbols)
                {
                    await symbolRepository.AddAsync(symbol);
                }
            }
            else
            {
                //update - similar implementation I've made for CurrencyRates, so I let myself to omit it for now.
            }        
        }
    }
}
