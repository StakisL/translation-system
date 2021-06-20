using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Serilog;
using TranslateSystem.Persistence;
using TranslateSystem.Persistence.Postgre;
using TranslateSystem.Persistence.Settings;
using TranslateSystemAPI.Scheduler;
using TranslateSystemAPI.Scheduler.Jobs;

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
            
            ConfigureScheduler(services);

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

        private void ConfigureScheduler(IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<JobRunner>();
            services.AddScoped<ExchangeRateRequestJob>();

            var jobs = SetJobSchedule();
            foreach (var job in jobs)
            {
                services.AddSingleton(job);
            }
            
            services.AddHostedService<SchedulerHostedService>();
        }
        
        private ICollection<JobSchedule> SetJobSchedule()
        {
            var configSection = Configuration.GetSection("Scheduler");
            var schedulerJobs = configSection.GetChildren()
                .Select(cs => new JobSchedule(
                    jobType: cs.Key.CurrentJob(),
                    cronExpression: cs.Value)).ToList();
            return schedulerJobs;
        }
    }
}
