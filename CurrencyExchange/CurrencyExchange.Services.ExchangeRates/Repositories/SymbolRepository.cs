using CurrencyExchange.ExchangeRates.Persistence;
using CurrencyExchange.Services.ExchangeRates.Entities;

namespace CurrencyExchange.Services.ExchangeRates.Repositories
{
    public class SymbolRepository : BaseRepository<Symbol>, ISymbolRepository
    {
        public SymbolRepository(ExchangeRatesDbContext dbContext) : base(dbContext)
        {
        }
    }
}
