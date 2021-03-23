using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TranslateSystemAPI.Models;

namespace TranslateSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRateGettingController : ControllerBase
    {

        [HttpGet]
        public CurrentExchangeRate GetCurrentExchangeCurrencyRate()
        {
            using var webClient = new WebClient();
            try
            {
                var json = webClient.DownloadString("https://api.exchangeratesapi.io/latest");
                var result = JsonConvert.DeserializeObject<CurrentExchangeRate>(json);
                Log.Information("Success load currency rates");
                return result;
            }
            catch (Exception e)
            {
                Log.Warning($"Current Exchange currency rate does not update", e.Message);
            }

            return null;
        }
    }
}
