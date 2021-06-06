using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslateSystem.Data;

namespace TranslateSystem.Persistence.Configurations
{
    public class CurrentExchangeRateRequestConfiguration : IEntityTypeConfiguration<CurrentExchangeRateRequest>
    {
        public void Configure(EntityTypeBuilder<CurrentExchangeRateRequest> builder)
        {
            builder.ToTable("current_exchange_rate_request");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Date).HasColumnName("date");
        }
    }
}
