namespace CurrencyExchange.Services.ExchangeRates.Entities
{
    public class AuditableEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
