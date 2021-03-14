using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslateSystem.Data;

namespace TranslateSystem.Persistence.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("currency");
            builder.HasKey(k => k.Id);
            
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.CurrencyType).HasColumnName("currency_type");
            builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        }
    }
}