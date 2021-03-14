using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslateSystem.Data;

namespace TranslateSystem.Persistence.Configurations
{
    public class TransferDataConfiguration : IEntityTypeConfiguration<TransferData>
    {
        public void Configure(EntityTypeBuilder<TransferData> builder)
        {
            builder.ToTable("transfer_data");
            builder.HasKey(k => k.Id);
            
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.SourceUserId).HasColumnName("source_id");
            builder.Property(p => p.DestinationUserId).HasColumnName("destination_id");

            builder.Property(p => p.SourceCurrencyType)
                   .HasConversion(v => v.ToString(), v => (CurrencyType)Enum.Parse(typeof(CurrencyType), v))
                   .HasColumnName("source_currency");

            builder.Property(p => p.DestinationCurrencyType)
                   .HasConversion(v => v.ToString(), v => (CurrencyType)Enum.Parse(typeof(CurrencyType), v))
                   .HasColumnName("destination_currency");

            builder.Property(p => p.AmountTransaction).HasColumnName("amount_transaction");
        }
    }
}