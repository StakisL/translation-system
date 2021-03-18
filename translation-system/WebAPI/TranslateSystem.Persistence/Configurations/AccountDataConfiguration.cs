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
            builder.Property(p => p.UserId).HasColumnName("user_id");
            builder.Property(p => p.CurrencyId).HasColumnName("currency_id");
            builder.Property(p => p.Balance).HasColumnName("balance");
        }
    }
}