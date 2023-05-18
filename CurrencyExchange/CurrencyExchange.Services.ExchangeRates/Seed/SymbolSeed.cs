using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Models;
using CurrencyExchange.Services.ExchangeRates.Repositories;
using CurrencyExchange.Services.ExchangeRates.Services;

namespace CurrencyExchange.Services.ExchangeRates.Seed
{
    public class SymbolSeed
    {
        public static async Task SeedAsync(IFixerApiService fixerService, IAsyncRepository<Symbol> symbolRepository, IMapper mapper)
        {
            IReadOnlyList<Symbol> mySymbols = null;
            SymbolsResponse symbolResponse = null;

            try
            {
               mySymbols = await symbolRepository.ListAllAsync();                
               symbolResponse = await fixerService.GetSymbols();
            }
            catch (Exception e)
            {
             //log   
            }

            if(symbolResponse == null || mySymbols == null || mySymbols.Count == symbolResponse.Symbols.Count)
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
