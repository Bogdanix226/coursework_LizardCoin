using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CryptoTracker.Areas.Identity.Data;
using CryptoTracker.Models;
using CryptoTracker.Services;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CryptoTracker.Controllers
{
    [Authorize] 
    public class PortfolioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CryptoService _cryptoService;

        public PortfolioController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, CryptoService cryptoService)
        {
            _context = context;
            _userManager = userManager;
            _cryptoService = cryptoService;
        }

        public async Task<IActionResult> Index(string currency = "usd")
        {
            var userId = _userManager.GetUserId(User);

            var userAssets = await _context.UserAssets
                .Where(ua => ua.ApplicationUserId == userId)
                .ToListAsync();

            var coinIds = userAssets.Select(ua => ua.CoinId).ToList();

            var marketData = await _cryptoService.GetSpecificCryptoDataAsync(coinIds, currency);

            var portfolioAssets = new List<PortfolioAssetViewModel>();
            decimal totalValue = 0;

            foreach (var asset in userAssets)
            {
                var marketInfo = marketData.FirstOrDefault(m => m.Id == asset.CoinId);
                if (marketInfo != null)
                {
                    decimal value = asset.Amount * marketInfo.CurrentPrice;
                    portfolioAssets.Add(new PortfolioAssetViewModel
                    {
                        CoinId = asset.CoinId,
                        Name = marketInfo.Name,
                        Symbol = marketInfo.Symbol.ToUpper(),
                        AmountOwned = asset.Amount,
                        CurrentPrice = marketInfo.CurrentPrice,
                        TotalValue = value
                    });
                    totalValue += value;
                }
            }

            var addableCoins = await _cryptoService.GetCryptoDataAsync(currency);

            var viewModel = new PortfolioViewModel
            {
                Assets = portfolioAssets.OrderByDescending(a => a.TotalValue).ToList(), 
                TotalPortfolioValue = totalValue,
                AddableCoins = addableCoins 
            };

            ViewBag.CurrentCurrency = currency;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsset(string coinId, decimal amount)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Будь ласка, введіть коректну кількість.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(coinId) || amount < 0)
            {
                TempData["ErrorMessage"] = "Невірні дані.";
                return RedirectToAction("Index");
            }

            var userId = _userManager.GetUserId(User);
            var existingAsset = await _context.UserAssets
                .FirstOrDefaultAsync(ua => ua.ApplicationUserId == userId && ua.CoinId == coinId);

            string successMessage = "";  

            if (existingAsset != null)
            {
                if (amount == 0)
                {
                    _context.UserAssets.Remove(existingAsset);
                    successMessage = $"Актив {coinId.ToUpper()} видалено з портфеля.";
                }
                else
                {
                    existingAsset.Amount = amount;
                    successMessage = $"Кількість {coinId.ToUpper()} оновлено до {amount}.";
                }
            }
            else if (amount > 0)
            {
                var newAsset = new UserAsset { ApplicationUserId = userId, CoinId = coinId, Amount = amount };
                _context.UserAssets.Add(newAsset);
                successMessage = $"{amount} {coinId.ToUpper()} додано до портфеля.";
            }
            else
            {
                successMessage = "Нічого не змінено (кількість 0).";
            }


            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = successMessage;

            return RedirectToAction("Index");
        }
    }
}