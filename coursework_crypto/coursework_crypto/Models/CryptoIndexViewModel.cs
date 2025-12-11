using System.Collections.Generic;

namespace CryptoTracker.Models
{
    public class CryptoIndexViewModel
    {
        public List<CryptoCurrency> Cryptos { get; set; }

        public HashSet<string> FavoriteCoinIds { get; set; }
    }
}