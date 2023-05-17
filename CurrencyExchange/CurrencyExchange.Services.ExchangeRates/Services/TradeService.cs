using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Repositories;

namespace CurrencyExchange.Services.ExchangeRates.Services
{
    public class TradeService : ITradeService
    {
        private readonly IAsyncRepository<Entities.Trade> tradeRepository;
        private readonly ICurrencyRateRepository currencyRateRepository;
        private readonly IFixerApiService fixerApiService;
        private readonly IMapper mapper;

        public TradeService(ITradeRepository tradeRepository, ICurrencyRateRepository currencyRateRepository, IFixerApiService fixerApiService,
            IMapper mapper)
        {
            this.tradeRepository = tradeRepository;
            this.currencyRateRepository = currencyRateRepository;
            this.fixerApiService = fixerApiService;
            this.mapper = mapper;
        }

        public async Task<decimal> MakeTrade(Models.Trade trade, decimal rate)
        {
            var tradeToDb = mapper.Map<Entities.Trade>(trade);
            tradeToDb.Rate = rate;
            tradeToDb.AmountTo = CalculateTrade(trade, rate);
            var added = await tradeRepository.AddAsync(tradeToDb);
            return added.AmountTo;

        }

        public async Task<decimal> RetrieveRate(string baseCurrency, string currencyTo)
        {
            var rate = await currencyRateRepository.RetrieveRate(baseCurrency, currencyTo);

            if (!rate.Equals(Decimal.Zero))
                return rate;
            else
            {
                return await fixerApiService.AddRate(baseCurrency, currencyTo);                
            }
        }

        private decimal CalculateTrade(Models.Trade trade, decimal rate)
        {
            return trade.Amount * rate;
        }
    }
}
