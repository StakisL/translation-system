using System.Threading.Tasks;
using Quartz;
using TranslateSystemAPI.Services;

namespace TranslateSystemAPI.Scheduler.Jobs
{
    public class ExchangeRateRequestJob : IJob
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateRequestJob(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }
        
        public async Task Execute(IJobExecutionContext context) => await _exchangeRateService.ExchangeRateRequest();
    }
}