using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Repositories;
using CurrencyExchange.Services.ExchangeRates.Services;

namespace CurrencyExchange.Services.ExchangeRates.Seed
{
    public class SymbolSeed
    {
        private const string criticalMessage = "Third-party API servers are down or your database. Restart app to seed/update database with Symbols.";

        public static async Task SeedAsync(IFixerApiService fixerService, IAsyncRepository<Symbol> symbolRepository,
            IMapper mapper, ILogger<SymbolSeed> logger)
        {
            try
            {
                var mySymbols = await symbolRepository.ListAllAsync();
                var symbolResponse = await fixerService.GetSymbols();
                var symbols = mySymbols.Select(obj => obj.SymbolTag).ToHashSet();
                var symbolsResponed = symbolResponse.Symbols.Keys.ToHashSet();
                var diff = symbolsResponed.Except(symbols);

                if(diff.Any()) //update-new or init
                {
                    var newSymbols = mapper.Map<List<Symbol>>(diff);

                    foreach (var symbol in newSymbols)
                    {
                        await symbolRepository.AddAsync(symbol);
                    }
                }
                else //up-to-date
                {
                    return;
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(criticalMessage, e);
            }
        }
    }
}
