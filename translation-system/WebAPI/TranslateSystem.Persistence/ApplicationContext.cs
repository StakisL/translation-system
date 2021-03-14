using Microsoft.EntityFrameworkCore;
using TranslateSystem.Data;
using TranslateSystem.Persistence.Configurations;

namespace TranslateSystem.Persistence
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<TransferData> TransferDatas { get; set; }
        public DbSet<AccountData> AccountDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new AccountDataConfiguration());
            modelBuilder.ApplyConfiguration(new TransferDataConfiguration());
        }
    }
}