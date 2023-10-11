using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Services;
using MyAnimeVault.Models;
using System.Diagnostics;

namespace MyAnimeVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnimeApiService AnimeApiService;

        private List<Anime> AnimeList = new List<Anime>();

        public HomeController(ILogger<HomeController> logger, IAnimeApiService animeApiService)
        {
            _logger = logger;
            AnimeApiService = animeApiService;

            UpdateAnimeList(); 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async void UpdateAnimeList()
        {
            AnimeList = await AnimeApiService.GetAllAnime();
        }
    }
}