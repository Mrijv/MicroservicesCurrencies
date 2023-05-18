using CurrencyExchange.ExchangeRates.Persistence;
using CurrencyExchange.Services.ExchangeRates.Entities;

namespace CurrencyExchange.Services.ExchangeRates.Repositories
{
    public class TradeRepository : BaseRepository<Trade>, ITradeRepository
    {
        public TradeRepository(ExchangeRatesDbContext dbContext) : base(dbContext)
        {
        }
    }
}
