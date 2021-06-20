using System.Threading.Tasks;
using Quartz;

namespace TranslateSystemAPI.Scheduler.Jobs
{
    public class ExchangeRateRequestJob : IJob
    {
        public ExchangeRateRequestJob()
        {
            
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}