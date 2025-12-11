using System.Text.Json.Serialization;

namespace CryptoTracker.Models
{
    public class CryptoCurrency
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("current_price")]
        public decimal CurrentPrice { get; set; }

        [JsonPropertyName("price_change_percentage_24h")]
        public decimal PriceChangePercentage24h { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
