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

        // This method gets called by the runtime. Use this method to add services to the container.
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            //todo add console log to migrations
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
