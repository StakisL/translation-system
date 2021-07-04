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
        public DbSet<TransferDataRequest> TransferData { get; set; }
        public DbSet<CurrentExchangeRateRequest> CurrentExchangeRateRequests { get; set; }
        public DbSet<AccountData> AccountData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new AccountDataConfiguration());
            modelBuilder.ApplyConfiguration(new TransferDataRequestConfiguration());
            modelBuilder.ApplyConfiguration(new CurrentExchangeRateRequestConfiguration());
        }
    }
}