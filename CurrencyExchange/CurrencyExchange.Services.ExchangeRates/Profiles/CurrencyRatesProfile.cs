using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Models;

namespace CurrencyExchange.Services.ExchangeRates.Profiles
{
    public class CurrencyRatesProfile : Profile
    {
        public CurrencyRatesProfile()
        {
            CreateMap<BaseCurrencyRate, List<CurrencyRate>>()
                .ConvertUsing<RatesConverter>();
            CreateMap<BaseCurrencyRate, CurrencyRate>()
                .ForMember(dest => dest.CurrencyTo, act => act.MapFrom(src => src.Rates.Keys.FirstOrDefault()));
        }
    }
}
