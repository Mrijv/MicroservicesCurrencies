using CurrencyExchange.ExchangeRates.Persistence;
using CurrencyExchange.Services.ExchangeRates.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Services.ExchangeRates.Repositories
{
    public class CurrencyRateRepository : BaseRepository<CurrencyRate>, ICurrencyRateRepository
    {
        private readonly ExchangeRatesDbContext dbContext;

        public CurrencyRateRepository(ExchangeRatesDbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<decimal> RetrieveRate(string currencyFrom, string currencyTo)
        {
            decimal result;

            try
            {
                result = await dbContext.Set<CurrencyRate>()
                .Where(row => row.BaseCurrency == currencyFrom && row.CurrencyTo == currencyTo)
                .Select(row => row.Rate)
                .FirstAsync();
            }
            catch (Exception)
            {
                return Decimal.Zero;
            }

            return result;
        }

        public async Task UpdateRateAsync(CurrencyRate currencyRate)
        {
            var entityToUpdate = await dbContext.Set<CurrencyRate>()
                .FirstOrDefaultAsync(row => row.BaseCurrency == currencyRate.BaseCurrency && row.CurrencyTo == currencyRate.CurrencyTo);
            
            if (entityToUpdate != null)
            {
                entityToUpdate.Rate = currencyRate.Rate;
                dbContext.SaveChanges();
            }
        }

        public async Task<CurrencyRate> ReturnAny()
        {
            return await dbContext.Set<CurrencyRate>().FirstOrDefaultAsync();
        }
    }
}
