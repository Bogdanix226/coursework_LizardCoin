using System.Net.Http;
using System.Text.Json;
using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoTracker.Services
{
    public class CryptoService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public CryptoService(HttpClient httpClient, IMemoryCache cache)
        {
            _cache = cache;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoTrackerApp");
        }

        //public async Task<List<CryptoCurrency>> GetCryptoDataAsync(string currency)
        //{
        //    string url = $"https://api.coingecko.com/api/v3/coins/markets" +
        //                 $"?vs_currency={currency}&order=market_cap_desc&per_page=50&page=1&sparkline=false";

        //    var response = await _httpClient.GetAsync(url);
        //    response.EnsureSuccessStatusCode();
        //    var json = await response.Content.ReadAsStringAsync();
        //    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //    return JsonSerializer.Deserialize<List<CryptoCurrency>>(json, options) ?? new List<CryptoCurrency>();
        //}
        /// <summary>
        /// Отримує список криптовалют для головної сторінки (з кешуванням).
        /// </summary>
        public async Task<List<CryptoCurrency>> GetCryptoDataAsync(string currency)
        {
            string cacheKey = $"CryptoData_{currency}";

            if (_cache.TryGetValue(cacheKey, out List<CryptoCurrency> cachedData))
            {
                return cachedData; 
            }

            string url = $"https://api.coingecko.com/api/v3/coins/markets" +
                         $"?vs_currency={currency}&order=market_cap_desc&per_page=50&page=1&sparkline=false";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var cryptos = JsonSerializer.Deserialize<List<CryptoCurrency>>(json, options) ?? new List<CryptoCurrency>();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            _cache.Set(cacheKey, cryptos, cacheOptions);

            return cryptos;
        }


        //public async Task<List<object[]>> GetOhlcDataAsync(string cryptoId, string currency, string timeRange)
        //{
        //    string days;
        //    switch (timeRange)
        //    {
        //        case "7d":
        //            days = "7";
        //            break;
        //        case "24h":
        //        default:
        //            days = "1";
        //            break;
        //    }

        //    string ohlcCurrency = "usd";

        //    string url = $"https://api.coingecko.com/api/v3/coins/{cryptoId}/ohlc" +
        //                 $"?vs_currency={ohlcCurrency}&days={days}";

        //    var response = await _httpClient.GetAsync(url);
        //    response.EnsureSuccessStatusCode();

        //    var json = await response.Content.ReadAsStringAsync();
        //    var ohlcData = JsonSerializer.Deserialize<List<object[]>>(json);

        //    return ohlcData ?? new List<object[]>();
        //}
        /// <summary>
        /// Отримує дані OHLC для свічкового графіка (з кешуванням).
        /// </summary>
        public async Task<List<object[]>> GetOhlcDataAsync(string cryptoId, string currency, string timeRange)
        {
            string cacheKey = $"OhlcData_{cryptoId}_{timeRange}";

            if (_cache.TryGetValue(cacheKey, out List<object[]> cachedData))
            {
                return cachedData; 
            }

            string days;
            switch (timeRange)
            {
                case "7d": days = "7"; break;
                case "24h":
                default: days = "1"; break;
            }

            string ohlcCurrency = "usd"; 
            string url = $"https://api.coingecko.com/api/v3/coins/{cryptoId}/ohlc" +
                         $"?vs_currency={ohlcCurrency}&days={days}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); 

            var json = await response.Content.ReadAsStringAsync();
            var ohlcData = JsonSerializer.Deserialize<List<object[]>>(json);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            _cache.Set(cacheKey, ohlcData, cacheOptions);

            return ohlcData ?? new List<object[]>();
        }

        /// <summary>
        /// Отримує дані для конкретного списку ID монет.
        /// </summary>
        public async Task<List<CryptoCurrency>> GetSpecificCryptoDataAsync(List<string> coinIds, string currency)
        {
            if (coinIds == null || !coinIds.Any())
            {
                return new List<CryptoCurrency>();
            }

            string idsParam = string.Join(",", coinIds);

            string url = $"https://api.coingecko.com/api/v3/coins/markets" +
                         $"?vs_currency={currency}&ids={idsParam}&order=market_cap_desc&sparkline=false";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<CryptoCurrency>>(json, options) ?? new List<CryptoCurrency>();
        }

        /// <summary>
        /// Отримує детальну інформацію про конкретну монету.
        /// </summary>
        //public async Task<CoinDetail> GetCoinDetailAsync(string coinId)
        //{
        //    string url = $"https://api.coingecko.com/api/v3/coins/{coinId}" +
        //                 "?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false";

        //    var response = await _httpClient.GetAsync(url);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return null; 
        //    }

        //    var json = await response.Content.ReadAsStringAsync();
        //    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        //    return JsonSerializer.Deserialize<CoinDetail>(json, options);
        //}
        
        public async Task<CoinDetail> GetCoinDetailAsync(string coinId)
        {
            string cacheKey = $"CoinDetail_{coinId}";

            if (_cache.TryGetValue(cacheKey, out CoinDetail cachedData))
            {
                return cachedData; 
            }

            string url = $"https://api.coingecko.com/api/v3/coins/{coinId}" +
                         "?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var coinInfo = JsonSerializer.Deserialize<CoinDetail>(json, options);

            if (coinInfo != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                _cache.Set(cacheKey, coinInfo, cacheOptions);
            }

            return coinInfo;
        }

        /// <summary>
        /// Fetches the latest cryptocurrency news from CoinGecko.
        /// </summary>
        public async Task<List<NewsArticle>> GetNewsAsync(int page = 1, int perPage = 20)
        {
            string url = $"https://api.coingecko.com/api/v3/news?page={page}&per_page={perPage}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching news: {response.StatusCode}");
                return new List<NewsArticle>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var apiResponse = JsonSerializer.Deserialize<NewsApiResponse>(json, options);

            return apiResponse?.Data ?? new List<NewsArticle>();
        }
        /// <summary>
        /// Fetches global cryptocurrency market data, including BTC dominance.
        /// </summary>
        //public async Task<GlobalData> GetGlobalDataAsync()
        //{
        //    string url = "https://api.coingecko.com/api/v3/global";

        //    var response = await _httpClient.GetAsync(url);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        Console.WriteLine($"Error fetching global data: {response.StatusCode}");
        //        return null; 
        //    }

        //    var json = await response.Content.ReadAsStringAsync();
        //    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        //    var apiResponse = JsonSerializer.Deserialize<GlobalApiResponse>(json, options);

        //    return apiResponse?.Data;
        //}
      
        public async Task<GlobalData> GetGlobalDataAsync()
        {
            string cacheKey = "GlobalData"; 

            if (_cache.TryGetValue(cacheKey, out GlobalData cachedData))
            {
                return cachedData; 
            }

            // Якщо в кеші немає - запит до API
            string url = "https://api.coingecko.com/api/v3/global";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<GlobalApiResponse>(json, options);
            var globalData = apiResponse?.Data;

            // Зберігаємо в кеш на 1 хвилину
            if (globalData != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                _cache.Set(cacheKey, globalData, cacheOptions);
            }

            return globalData;
        }
    }
}
 