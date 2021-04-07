using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TranslateSystemAPI.Models
{
    public class CurrentExchangeRate
    {
        public bool Success { get; set; }

        public DateTime Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double> ExchangeRates { get; set; }
    }
}
