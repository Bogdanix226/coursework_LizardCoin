using System.Collections.Generic;

namespace CryptoTracker.Models
{
    public class ChartViewModel
    {
        // Дані для свічкового графіка (OHLC)
        public List<object[]> OhlcData { get; set; }

        // Детальна інформація про монету
        public CoinDetail CoinInfo { get; set; }

        // Поточна обрана валюта 
        public string CurrentCurrency { get; set; }
    }
}