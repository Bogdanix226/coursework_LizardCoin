using System.Text.Json.Serialization;

namespace CryptoTracker.Models
{
    public class NewsApiResponse 
    {
        [JsonPropertyName("data")]
        public List<NewsArticle> Data { get; set; }
    }

    public class NewsArticle
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } 

        [JsonPropertyName("news_site")]
        public string NewsSite { get; set; } 

        [JsonPropertyName("thumb_2x")]
        public string ThumbnailUrl { get; set; } 

        [JsonPropertyName("updated_at")]
        public long UpdatedAtUnix { get; set; } 

        public DateTimeOffset UpdatedAt => DateTimeOffset.FromUnixTimeSeconds(UpdatedAtUnix);
    }
}