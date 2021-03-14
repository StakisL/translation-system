using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslateSystem.Data;

namespace TranslateSystem.Persistence.Configurations
{
    public class AccountDataConfiguration : IEntityTypeConfiguration<AccountData>
    {
        public void Configure(EntityTypeBuilder<AccountData> builder)
        {
            builder.ToTable("account_data");
            builder.HasKey(k => k.Id);
            
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.CurrencyType).HasColumnName("currency_type");
            builder.Property(p => p.Balance).HasColumnName("balance");
        }
    }
}