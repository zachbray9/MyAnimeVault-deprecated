using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Services;
using MyAnimeVault.Models;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace MyAnimeVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnimeApiService AnimeApiService;

        public List<AnimeListNode> AnimeList { get; set; } = new List<AnimeListNode>();

        public HomeController(ILogger<HomeController> logger, IAnimeApiService animeApiService)
        {
            _logger = logger;
            AnimeApiService = animeApiService;
        }

        public async Task<IActionResult> Index()
        {
            AnimeList = await AnimeApiService.GetAllAnime();
            return View(AnimeList);
        }

        public async Task<IActionResult> AnimeDetails(int id) 
        {
            Anime anime = await AnimeApiService.GetAnimeById(id);
            return View(anime);
        }

        public async Task<IActionResult> SearchResults(string query)
        {
            List<AnimeListNode> SearchResults = await AnimeApiService.GetListOfAnimeByQuery(query);
            return View(SearchResults);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}