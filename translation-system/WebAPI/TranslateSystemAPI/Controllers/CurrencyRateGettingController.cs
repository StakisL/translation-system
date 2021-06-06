using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using TranslateSystem.Data;
using TranslateSystem.Persistence;
using TranslateSystemAPI.Extensions;
using TranslateSystemAPI.Models;

namespace TranslateSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CurrencyRateGettingController : ControllerBase
    {
        private static readonly HttpClient HttpClient = new();

        private readonly string _downloadLink;
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _applicationContext;

        public CurrencyRateGettingController(IConfiguration configuration, ApplicationContext applicationContext)
        {
            _configuration = configuration;
            _applicationContext = applicationContext;
            _downloadLink = CreateDownloadLink();
        }

        //todo Переделать контроллер на ожидание вводных параметров из Swagger, а не appsettings
        [HttpGet]
        public async Task GetCurrentExchangeCurrencyRate()
        {
            try
            {
                using var response = HttpClient.GetAsync(_downloadLink).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<CurrentExchangeRate>(json);
                await SaveCurrenciesInDb(result);

                Log.Information($"Success load currency rates, {result}");
            }
            catch (Exception e)
            {
                Log.Warning("Current Exchange currency rate does not update", e.Message);
            }
        }

        private async Task SaveCurrenciesInDb(CurrentExchangeRate exchangeRate)
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
