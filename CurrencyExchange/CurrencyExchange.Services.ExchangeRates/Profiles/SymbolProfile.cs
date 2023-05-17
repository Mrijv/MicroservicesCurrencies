using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Models;

namespace CurrencyExchange.Services.ExchangeRates.Profiles
{
    public class SymbolProfile : Profile
    {
        public SymbolProfile()
        {
            CreateMap<SymbolsResponse, List<Symbol>>()
                .ConvertUsing<SymbolsConverter>();
        }
    }
}
