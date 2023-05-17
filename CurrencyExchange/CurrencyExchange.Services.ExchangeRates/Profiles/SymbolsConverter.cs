using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Models;

namespace CurrencyExchange.Services.ExchangeRates.Profiles
{
    public class SymbolsConverter : ITypeConverter<SymbolsResponse, List<Symbol>>
    {
        public List<Symbol> Convert(SymbolsResponse source, List<Symbol> destination, ResolutionContext context)
        {
            var symbols = source.Symbols;
            var result = new List<Symbol>();

            foreach (var symbol in symbols)
            {
                result.Add(new Symbol
                {
                    SymbolTag = symbol.Key
                });
            }

            return result;
        }
    }
}
