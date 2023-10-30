using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Services;
using MyAnimeVault.Models;
using MyAnimeVault.Services.Authentication;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace MyAnimeVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly IAuthenticator Authenticator;
        private readonly IAnimeApiService AnimeApiService;

        public List<AnimeListNode> AnimeList { get; set; } = new List<AnimeListNode>();

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IAuthenticator authenticator, IAnimeApiService animeApiService)
        {
            _logger = logger;
            HttpContextAccessor = httpContextAccessor;
            Authenticator = authenticator;
            AnimeApiService = animeApiService;
        }

        public async Task<IActionResult> Index()
        {
            StoreUserDataInSession();
            AnimeList = await AnimeApiService.GetAllAnime();
            return View(AnimeList);
        }

        public async Task<IActionResult> AnimeDetails(int id) 
        {
            StoreUserDataInSession();
            Anime anime = await AnimeApiService.GetAnimeById(id);
            return View(anime);
        }

        public async Task<IActionResult> SearchResults(string query)
        {
            StoreUserDataInSession();
            List<AnimeListNode> SearchResults = await AnimeApiService.GetListOfAnimeByQuery(query);
            return View(SearchResults);
        }

        public IActionResult Vault() 
        {
            StoreUserDataInSession();

            if(HttpContextAccessor.HttpContext.Session.GetString("FirebaseToken") == null)
            {
                return RedirectToAction("Index", "Login");
            }
                return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //helper methods
        private async void StoreUserDataInSession()
        {
            bool UserAlreadyAuthenticated = HttpContextAccessor.HttpContext.Session.GetString("FirebaseToken") != null ? true : false;
            bool AuthCookieExists = HttpContextAccessor.HttpContext.Request.Cookies["FirebaseToken"] != null ? true : false;
        
            if(UserAlreadyAuthenticated)
            {
                ViewBag.FirebaseToken = HttpContextAccessor.HttpContext.Session.GetString("FirebaseToken");
                ViewBag.UserId = HttpContextAccessor.HttpContext.Session.GetString("UserId");
                ViewBag.Email = HttpContextAccessor.HttpContext.Session.GetString("Email");
                ViewBag.DisplayName = HttpContextAccessor.HttpContext.Session.GetString("DisplayName");
            }
            else if (AuthCookieExists)
            {

                try
                {
                    AuthCredential authCredential = JsonConvert.DeserializeObject<AuthCredential>(HttpContextAccessor.HttpContext.Request.Cookies["FirebaseToken"]);

                    UserCredential userCredential = await Authenticator.LoginWithCredentialAsync(authCredential); ViewBag.FirebaseToken = userCredential.User.Credential.IdToken;
                    ViewBag.UserId = userCredential.User.Uid;
                    ViewBag.Email = userCredential.User.Info.Email;
                    ViewBag.DisplayName = userCredential.User.Info.DisplayName;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

    }
}