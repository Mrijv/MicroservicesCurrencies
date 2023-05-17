using CurrencyExchange.Services.ExchangeRates.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyExchange.Services.ExchangeRates.Configurations
{
    public class TradeConfiguration : IEntityTypeConfiguration<Trade>
    {
        public void Configure(EntityTypeBuilder<Trade> builder)
        {
            builder.Property(o => o.AmountFrom)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(o => o.AmountTo)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(o => o.Rate)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(o => o.Result)
                .HasColumnType("money");

            builder.Property(o => o.CurrencyFrom)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(o => o.CurrencyTo)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(o => o.UserId)
                .IsRequired();
        }
    }
}
