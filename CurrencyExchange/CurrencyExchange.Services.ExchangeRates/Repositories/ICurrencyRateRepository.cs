using CurrencyExchange.Services.ExchangeRates.Entities;

namespace CurrencyExchange.Services.ExchangeRates.Repositories
{
    public interface ICurrencyRateRepository : IAsyncRepository<CurrencyRate>
    {
    }
}
