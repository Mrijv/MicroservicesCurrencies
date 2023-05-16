using CurrencyExchange.ExchangeRates.Persistence;
using CurrencyExchange.Services.ExchangeRates.Entities;

namespace CurrencyExchange.Services.ExchangeRates.Repositories
{
    public class CurrencyRateRepository : BaseRepository<CurrencyRate>, ICurrencyRateRepository
    {
        public CurrencyRateRepository(ExchangeRatesDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
