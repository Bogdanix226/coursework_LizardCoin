using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace CryptoTracker.Models
{
    public class CoinDetail
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public Dictionary<string, string> Description { get; set; } 

        [JsonPropertyName("links")]
        public CoinLinks Links { get; set; }

        [JsonPropertyName("market_data")]
        public MarketData MarketData { get; set; }

        public string GetDescription(string lang = "en") => Description?.GetValueOrDefault(lang, "");
    }

    public class CoinLinks
    {
        [JsonPropertyName("homepage")]
        public List<string> Homepage { get; set; } 
        public string PrimaryHomepage => Homepage?.FirstOrDefault(link => !string.IsNullOrEmpty(link));
    }

    public class MarketData
    {
        [JsonPropertyName("current_price")]
        public Dictionary<string, decimal> CurrentPrice { get; set; } 

        [JsonPropertyName("market_cap")]
        public Dictionary<string, decimal> MarketCap { get; set; } 

        [JsonPropertyName("total_volume")]
        public Dictionary<string, decimal> TotalVolume { get; set; } 

        public decimal GetCurrentPrice(string currency) => CurrentPrice?.GetValueOrDefault(currency.ToLower(), 0) ?? 0;
        public decimal GetMarketCap(string currency) => MarketCap?.GetValueOrDefault(currency.ToLower(), 0) ?? 0;
        public decimal GetTotalVolume(string currency) => TotalVolume?.GetValueOrDefault(currency.ToLower(), 0) ?? 0;
    }
}