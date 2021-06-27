using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using TranslateSystem.Persistence.Settings;

namespace TranslateSystem.Persistence.Postgre
{
    public class PostgreSqlContextFactory : IContextFactory
    {
        private static readonly string MigrationAssembly = typeof(PostgreSqlContextFactory).Assembly.GetName().Name;
        private readonly DbContextOptions _dbContextOptions;

        public PostgreSqlContextFactory(IConnectionProvider connectionProvider)
        {
            _dbContextOptions = BuildOptions(connectionProvider);
        }

        public ApplicationContext CreateContext() => new (_dbContextOptions);

        public DbContextOptions BuildOptions(IConnectionProvider connectionProvider)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(connectionProvider.GetConnection(), Configure);
            
            return builder.Options;
        }

        private void Configure(NpgsqlDbContextOptionsBuilder builder)
        {
            builder.MigrationsAssembly(MigrationAssembly);
        }
    }
}
