using System;
using System.Collections.Generic;

namespace TranslateSystem.Data
{
    public class CurrentExchangeRateRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
        public ICollection<Currency> Currencies { get; set; }
    }
}
