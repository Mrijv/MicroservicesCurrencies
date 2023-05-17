using CurrencyExchange.Services.ExchangeRates.Entities;

namespace CurrencyExchange.Services.ExchangeRates.Repositories
{
    public interface ISymbolRepository : IAsyncRepository<Symbol>
    {
    }
}
