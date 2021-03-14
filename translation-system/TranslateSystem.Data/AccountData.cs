using System;

namespace TranslateSystem.Data
{
    public class AccountData
    {
        public int Id { get; set; }
        
        public int Balance { get; set; }
        
        public CurrencyType CurrencyType { get; set; }
    }
}
