using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace TranslateSystemAPI.Scheduler.Jobs
{
    public class JobRunner : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public JobRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var jobType = context.JobDetail.JobType;
            var job = (IJob)scope.ServiceProvider.GetRequiredService(jobType);

            await job.Execute(context);
        }
    }
}