using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyExchange.Services.ExchangeRates.Entities
{
    public class CurrencyRate :AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CurrencyRateId { get; set; }
        [MaxLength(10)]
        public string BaseCurrency { get; set; }
        [ForeignKey("BaseCurrency")]
        public Symbol? Symbol { get; set; }
        [Required]
        [MaxLength(10)]
        public string CurrencyTo { get; set; }
        [Required]
        public Decimal Rate { get; set; }
        [Required]
        public DateTime RetrievalDate { get; set; }        
        public int TimeStamp { get; set; }
    }
}
