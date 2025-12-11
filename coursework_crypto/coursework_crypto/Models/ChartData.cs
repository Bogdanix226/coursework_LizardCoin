using System.Text.Json.Serialization;

namespace CryptoTracker.Models
{
    public class ChartData
    {
        [JsonPropertyName("prices")]
        public List<List<double>> Prices { get; set; }
    }
}