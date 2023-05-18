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
                .ForMember(dest => dest.CurrencyTo, act => act.MapFrom(src => src.Rates.Keys.FirstOrDefault()))
                .ForMember(dest => dest.Rate, act => act.MapFrom(src => src.Rates.Values.FirstOrDefault()));
            CreateMap<BaseCurrencyOneRate, CurrencyRate>().ReverseMap();
            CreateMap<BaseCurrencyOneRate, BaseCurrencyRate>()
                .ReverseMap()
                .ForMember(dest => dest.Rate, act => act.MapFrom(src => src.Rates.Values.FirstOrDefault())); ;

        }
    }
}
