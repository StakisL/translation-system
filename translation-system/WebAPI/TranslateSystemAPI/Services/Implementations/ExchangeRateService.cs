using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using TranslateSystem.Data;
using TranslateSystem.Persistence;
using TranslateSystemAPI.Extensions;
using TranslateSystemAPI.Models;

namespace TranslateSystemAPI.Services.Implementations
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _applicationContext;
        private readonly string _downloadLink;
        
        public ExchangeRateService(
            HttpClient httpClient,
            IConfiguration configuration, 
            IContextFactory applicationContext)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _applicationContext = applicationContext.CreateContext();
            
            _downloadLink = CreateDownloadLink();
        }
        
        public async Task ExchangeRateRequest()
        {
            try
            {
                using var response = _httpClient.GetAsync(_downloadLink).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<ExchangeRate>(json);
                await SaveCurrenciesInDb(result);

                Log.Information($"Success load currency rates, {result}");
            }
            catch (Exception e)
            {
                Log.Error("Current Exchange currency rate does not update", e.Message);
            }
        }
        
        private async Task SaveCurrenciesInDb(ExchangeRate exchangeRate)
        {
            if (exchangeRate.Success)
            {
                var currencies = exchangeRate.ExchangeRates.Select(item => new Currency()
                {
                    CurrencyType = item.Key.GetCurrencyType(),
                    LastUpdate = exchangeRate.Date,
                    Ratio = item.Value
                }).ToList();

                await _applicationContext.AddAsync(new CurrentExchangeRateRequest()
                {
                    Date = DateTime.UtcNow,
                    Currencies = currencies
                });

                await _applicationContext.SaveChangesAsync();
            }
        }

        //Todo избавиться от этого метода.
        private string CreateDownloadLink()
        {
            var currencies = _configuration.GetSection("Currencies").AsEnumerable();

            var downloadLink = new StringBuilder()
                .Append("http://api.exchangeratesapi.io/v1/latest?access_key=")
                .Append($"{_configuration["ExchangeRateAPIKey"]}")
                .Append("&symbols=");

            foreach (var currency in currencies)
            {
                if (currency.Value != null)
                {
                    downloadLink.Append($"{currency.Value},");
                }
            }
            downloadLink.Remove(downloadLink.Length - 1, 1);

            return downloadLink.ToString();
        }
    }
}