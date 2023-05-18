using System.ComponentModel.DataAnnotations;

namespace CurrencyExchange.Services.ExchangeRates.Entities
{
    public class Trade : AuditableEntity
    {
        public Guid TradeId { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public decimal Rate { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public Guid UserId { get; set; }
    }
}
