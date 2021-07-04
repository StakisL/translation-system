using System;
using TranslateSystemAPI.Scheduler.Jobs;

namespace TranslateSystemAPI.Scheduler
{
    public static class JobExtension
    {
        public static Type CurrentJob(this string job)
        {
            return job switch
            {
                nameof(ExchangeRateRequestJob) => typeof(ExchangeRateRequestJob),
                _ => throw new ArgumentException("Wrong type of job!")
            };
        }
    }
}