using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using TranslateSystem.Persistence;
using TranslateSystem.Persistence.Postgre;
using TranslateSystem.Persistence.Settings;

namespace TranslateSystemAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers();
            services.AddSingleton<IContextFactory, PostgreSqlContextFactory>();
            services.AddSingleton<IConnectionProvider, BasicConnectionProvider>(sp =>
                new BasicConnectionProvider(Configuration["ConnectionString"]));

            services.AddScoped(sp => sp.GetRequiredService<IContextFactory>().CreateContext());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TranslateSystemAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            RunMigrations(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslateSystemAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RunMigrations(IApplicationBuilder app)
        {
            var contextFactory = app.ApplicationServices.GetService<IContextFactory>();
            using var context = contextFactory?.CreateContext();
            var pendingMigrations = context?.Database.GetPendingMigrations().ToList();
            if (pendingMigrations!.Any())
            {
                Log.Information($"Running {pendingMigrations.Count} DB migrations");
                foreach (var migration in pendingMigrations)
                {
                    Log.Information($"\t{migration}");
                }
            }
            else
            {
                Log.Information("No DB pending migrations found");
            }

            context?.Database.Migrate();
            Log.Information($"DB migration completed");
        }
    }
}
