using System.ComponentModel.DataAnnotations;

namespace CurrencyExchange.Services.ExchangeRates.Entities
{
    public class Symbol
    {
        [Key]
        [MaxLength(10)]
        public string SymbolTag { get; set; }
        public ICollection<CurrencyRate> Rates { get; set; }
    }
}
