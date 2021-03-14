using System;

namespace TranslateSystem.Data
{
    public class Operation
    {
        public int Id { get; set; }
        
        public int SourceUserId { get; set; }
        
        public int DestinationUserId { get; set; }
        
        public CurrencyType SourceCurrencyType { get; set; }
        
        public CurrencyType DestinationCurrencyType { get; set; }
        
        public double AmountTransaction { get; set; }
    }
}
