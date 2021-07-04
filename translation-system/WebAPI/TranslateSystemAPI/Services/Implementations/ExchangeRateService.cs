using System;
using System.Collections.Generic;
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
                Log.Error($"Current Exchange currency rate does not update, {e.Message}");
            }
        }
        
        private async Task SaveCurrenciesInDb(ExchangeRate exchangeRate)
        {
            if (exchangeRate.Success)
            {
                var currencies = AddOrUpdateCurrency(exchangeRate);
                
                await _applicationContext.AddAsync(new CurrentExchangeRateRequest()
                {
                    Date = DateTime.UtcNow,
                    Currencies = currencies
                });
                await _applicationContext.SaveChangesAsync();
            }
        }

        private ICollection<Currency> AddOrUpdateCurrency(ExchangeRate exchangeRate)
        {
            //TODO Разобраться и придумать нормальную реализацию AddOrUpdate.
            //todo Словарь как хранилище обновленных курсов, тоже явно не лучший вариант. (Переделать парсер)
            var currencies = new LinkedList<Currency>();
            foreach (var key in exchangeRate.ExchangeRates.Keys)
            {
                var result = _applicationContext.Currencies.FirstOrDefault(
                    c => c.CurrencyType == key.GetCurrencyType());
                    
                if (result == null)
                {
                    var currency = new Currency
                    {
                        LastUpdate = exchangeRate.Date,
                        CurrencyType = key.GetCurrencyType(),
                        Ratio = exchangeRate.ExchangeRates[key]
                    };
                        
                    currencies.AddFirst(currency);
                }
                else
                {
                    result.LastUpdate = exchangeRate.Date;
                    result.Ratio = exchangeRate.ExchangeRates[key];

                    currencies.AddFirst(result);
                    _applicationContext.Remove(result);
                }
            }

            return currencies;
        }
        
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