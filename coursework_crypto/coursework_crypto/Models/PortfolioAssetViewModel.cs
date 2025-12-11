namespace CryptoTracker.Models
{
    public class PortfolioAssetViewModel
    {
        public string CoinId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal AmountOwned { get; set; } 
        public decimal CurrentPrice { get; set; } 
        public decimal TotalValue { get; set; }
    }
}