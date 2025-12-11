using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace CryptoTracker.Models
{
    public class GlobalApiResponse
    {
        [JsonPropertyName("data")]
        public GlobalData Data { get; set; }
    }

    public class GlobalData
    {
        [JsonPropertyName("total_market_cap")]
        public Dictionary<string, decimal> TotalMarketCap { get; set; }

        [JsonPropertyName("market_cap_percentage")]
        public Dictionary<string, decimal> MarketCapPercentage { get; set; }

        public decimal BtcDominance => MarketCapPercentage?.GetValueOrDefault("btc", 0) ?? 0;

        public decimal GetTotalMarketCap(string currency) => TotalMarketCap?.GetValueOrDefault(currency.ToLower(), 0) ?? 0;
    }
}