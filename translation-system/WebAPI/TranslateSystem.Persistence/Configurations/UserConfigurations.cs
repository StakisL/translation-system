using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslateSystem.Data;

namespace TranslateSystem.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.FullName).HasColumnName("full_name");
            builder.Property(p => p.Email).HasColumnName("email");
            //todo Посмотреть как правильно хранят пароли в базе и хранят ли вообще!?
            builder.Property(p => p.Password).HasColumnName("password");
            
            builder.HasOne(p => p.AccountData)
                   .WithOne(ad => ad.User)
                   .HasForeignKey<AccountData>(ad => ad.UserId);

            builder.HasIndex(p => p.Email).IsUnique();
        }
    }
}