using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _applicationContext;

        public CurrencyRateGettingController(IConfiguration configuration, ApplicationContext applicationContext)
        {
            _configuration = configuration;
            _applicationContext = applicationContext;
        }

        [HttpGet]
        public async Task GetCurrentExchangeCurrencyRate()
        {
            using var webClient = new WebClient();
            try
            {
                var json = webClient.DownloadString(CreateDownloadLink());
                var result = JsonConvert.DeserializeObject<CurrentExchangeRate>(json);
                if (result.Success)
                {
                    var currencies = result.ExchangeRates.Select(item => new Currency() 
                        {
                            CurrencyType = item.Key.GetCurrencyType(), 
                            LastUpdate = result.Date, 
                            Ratio = item.Value
                        }).ToList();

                    await _applicationContext.AddAsync(new CurrentExchangeRateRequest()
                    {
                        Date = result.Date,
                        Currencies = currencies
                    });
                    await _applicationContext.SaveChangesAsync();
                }
                Log.Information($"Success load currency rates, {result}");
            }
            catch (Exception e)
            {
                Log.Warning($"Current Exchange currency rate does not update", e.Message);
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
