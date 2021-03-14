using System;

namespace TranslateSystem.Data
{
    public enum CurrencyType : int
    {
        EUR, 
        USD, 
        GPB, 
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
