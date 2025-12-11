using System.Collections.Generic;

namespace CryptoTracker.Models
{
    public class PortfolioViewModel
    {
        public List<PortfolioAssetViewModel> Assets { get; set; }

        public decimal TotalPortfolioValue { get; set; }

        public List<CryptoCurrency> AddableCoins { get; set; }
    }
}