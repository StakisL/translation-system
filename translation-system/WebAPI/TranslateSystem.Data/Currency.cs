using System;

namespace TranslateSystem.Data
{
    public enum CurrencyType
    {
        EUR, 
        USD, 
        JPY, 
        RUB, 
        BTC
    }
    
    public class Currency
    {
        public int Id { get; set; }
        public CurrencyType CurrencyType { get; set;}
        public double Ratio { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
