using Microsoft.AspNetCore.Mvc;
using CryptoTracker.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq; 
using System.Collections.Generic; 
using Microsoft.AspNetCore.Identity; 
using CryptoTracker.Areas.Identity.Data; 
using Microsoft.EntityFrameworkCore; 
using CryptoTracker.Models; 
using Microsoft.AspNetCore.Authorization; 

namespace CryptoTracker.Controllers
{
    public class CryptoController : Controller
    {
        private readonly CryptoService _cryptoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CryptoController(CryptoService cryptoService,
                                IConfiguration configuration,
                                UserManager<ApplicationUser> userManager,
                                ApplicationDbContext context)
        {
            _cryptoService = cryptoService;
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchString, string currency = "usd", string sortBy = "")
        {
            ViewData["CurrentFilter"] = searchString;
            ViewBag.CurrentCurrency = currency;
            ViewBag.CurrentSort = sortBy;

            var cryptos = await _cryptoService.GetCryptoDataAsync(currency);
            var globalData = await _cryptoService.GetGlobalDataAsync();

            ViewBag.GlobalData = globalData;

            if (!String.IsNullOrEmpty(searchString))
            {
                cryptos = cryptos.Where(c =>
                    c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    c.Symbol.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            switch (sortBy)
            {
                case "growth": cryptos = cryptos.OrderByDescending(c => c.PriceChangePercentage24h).ToList(); break;
                case "fall": cryptos = cryptos.OrderBy(c => c.PriceChangePercentage24h).ToList(); break;
                default: break;
            }

            var favoritedIds = new HashSet<string>();

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);

                var favoriteList = await _context.UserFavorites
                    .Where(f => f.ApplicationUserId == userId)
                    .Select(f => f.CoinId)
                    .ToListAsync(); 

                favoritedIds = new HashSet<string>(favoriteList);
            }

            var viewModel = new CryptoIndexViewModel
            {
                Cryptos = cryptos,
                FavoriteCoinIds = favoritedIds
            };

            return View(viewModel);
        }


        [Authorize] 
        public async Task<IActionResult> Favorites(string currency = "usd")
        {
            ViewBag.CurrentCurrency = currency;

            var userId = _userManager.GetUserId(User);

            var favoriteCoinIds = await _context.UserFavorites
                .Where(f => f.ApplicationUserId == userId)
                .Select(f => f.CoinId)
                .ToListAsync(); 

            var cryptos = await _cryptoService.GetSpecificCryptoDataAsync(favoriteCoinIds, currency);

            var viewModel = new CryptoIndexViewModel
            {
                Cryptos = cryptos,
                FavoriteCoinIds = new HashSet<string>(favoriteCoinIds)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> ToggleFavorite(string coinId)
        {
            if (string.IsNullOrEmpty(coinId))
            {
                return Json(new { success = false, message = "Coin ID not provided." });
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var favorite = await _context.UserFavorites
                .FirstOrDefaultAsync(f => f.ApplicationUserId == userId && f.CoinId == coinId);

            if (favorite == null)
            {
                var newFavorite = new UserFavorite
                {
                    ApplicationUserId = userId,
                    CoinId = coinId
                };
                _context.UserFavorites.Add(newFavorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, status = "added" });
            }
            else
            {
                _context.UserFavorites.Remove(favorite);
                await _context.SaveChangesAsync();
                return Json(new { success = true, status = "removed" });
            }
        }


        public async Task<IActionResult> Chart(string id, string currency = "usd", string timeRange = "24h")
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var ohlcData = await _cryptoService.GetOhlcDataAsync(id, currency, timeRange); 

            var coinInfo = await _cryptoService.GetCoinDetailAsync(id);

            if (coinInfo == null)
            {
                TempData["ErrorMessage"] = $"Монету з ID '{id}' не знайдено.";
                return RedirectToAction("Index");
            }

            var viewModel = new ChartViewModel
            {
                OhlcData = ohlcData,
                CoinInfo = coinInfo,
                CurrentCurrency = currency 
            };

            ViewBag.Symbol = coinInfo.Symbol.ToUpper(); 
            ViewBag.CurrentCurrency = currency.ToUpper();
            ViewBag.CurrentTimeRange = timeRange;

            return View(viewModel);
        }
    }
}