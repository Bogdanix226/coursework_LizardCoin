using Microsoft.AspNetCore.Mvc;
using CryptoTracker.Services;
using System.Threading.Tasks;

namespace CryptoTracker.Controllers
{
    public class NewsController : Controller
    {
        private readonly CryptoService _cryptoService;

        public NewsController(CryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        public async Task<IActionResult> Index()
        {
            var newsArticles = await _cryptoService.GetNewsAsync();
            return View(newsArticles);
        }
    }
}