using System;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using TranslateSystem.Persistence.Settings;

namespace TranslateSystem.Persistence.Postgre
{
    public class PostgreSqlMigrationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var host = Environment.GetEnvironmentVariable("DEV_PGSQL_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DEV_PGSQL_PORT") ?? "5434";
            var username = Environment.GetEnvironmentVariable("DEV_PGSQL_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("DEV_PGSQL_PASSWORD");

            var builder = new NpgsqlConnectionStringBuilder()
            {
                Host = host,
                Port = Int32.Parse(port),
                Username = username,
                Password = password,
                Database = "translation-system"
            };

            var cs = new BasicConnectionProvider(builder.ToString());
            var factory = new PostgreSqlContextFactory(cs);
            return factory.CreateContext();
        }
    }
}
