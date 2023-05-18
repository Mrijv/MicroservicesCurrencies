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
            CreateMap<IEnumerable<string>, List<Symbol>>()
                .ConvertUsing(source => source.Select(s => new Symbol { SymbolTag = s }).ToList());
        }
    }
}
