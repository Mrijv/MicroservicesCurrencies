using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Models;

namespace CurrencyExchange.Services.ExchangeRates.Profiles
{
    public class RatesConverter : ITypeConverter<BaseCurrencyRate, List<CurrencyRate>>
    {
        public List<CurrencyRate> Convert(BaseCurrencyRate source, List<CurrencyRate> destination, ResolutionContext context)
        {
            var rates = source.Rates;
            var result = new List<CurrencyRate>();

            foreach (var rate in rates)
            {
                result.Add(new CurrencyRate { 
                    CurrencyTo = rate.Key,
                    BaseCurrency = source.BaseCurrency,
                    Rate = rate.Value,
                    RetrievalDate = source.RetrievalDate,
                    TimeStamp = source.TimeStamp
                });
            }

            return result;
        }
    }
}
