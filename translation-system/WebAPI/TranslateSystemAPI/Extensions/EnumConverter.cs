using System;
using TranslateSystem.Data;

namespace TranslateSystemAPI.Extensions
{
    public static class EnumConverter
    {
        public static CurrencyType GetCurrencyType(this string currency)
        {
            return currency switch
            {
                "USD" => CurrencyType.USD,
                "RUB" => CurrencyType.RUB,
                "EUR" => CurrencyType.EUR,
                "BTC" => CurrencyType.BTC,
                "JPY" => CurrencyType.JPY,
                _ => throw new ArgumentOutOfRangeException(nameof(currency), currency, null)
            };
        }
    }
}
