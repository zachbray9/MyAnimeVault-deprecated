using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Services;
using MyAnimeVault.Models;
using MyAnimeVault.Services.Authentication;
using System.Diagnostics;

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

            if(HttpContextAccessor.HttpContext?.Session.GetString("UserId") == null)
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
        private async void StoreUserDataInSession() //checks if the user is already logged in or has a session cookie. If not, user must login
        {
            bool UserAlreadyAuthenticated = HttpContextAccessor.HttpContext?.Session.GetString("UserId") != null ? true : false;
            string? sessionCookie = HttpContextAccessor.HttpContext?.Request.Cookies["Session"];
        
            if(UserAlreadyAuthenticated) //checks if the user has already logged in during this session (The session variables are populated)
            {
                ViewBag.UserId = HttpContextAccessor.HttpContext?.Session.GetString("UserId");
                ViewBag.Email = HttpContextAccessor.HttpContext?.Session.GetString("Email");
                ViewBag.DisplayName = HttpContextAccessor.HttpContext?.Session.GetString("DisplayName");
            }
            else if (!string.IsNullOrEmpty(sessionCookie)) //checks if there is a session cookie
            {

                try
                {
                    FirebaseToken decodedToken = await Authenticator.VerifyCookieAsync(sessionCookie); //checks if the session cookie is still valid
                    if(decodedToken.Uid != null)
                    {
                        UserRecord userRecord = await Authenticator.GetUserByUidAsync(decodedToken.Uid);
                        ViewBag.UserId = userRecord.Uid;
                        ViewBag.Email = userRecord.Email;
                        ViewBag.DisplayName = userRecord.DisplayName;

                    }
                }
                catch (FirebaseAuthException ex)
                {
                    switch(ex.AuthErrorCode)
                    {
                        case AuthErrorCode.ExpiredSessionCookie:
                            HttpContextAccessor.HttpContext?.Response.Cookies.Delete("Session");
                            Debug.WriteLine("The specified session cookie is expired.");
                            break;
                        case AuthErrorCode.InvalidSessionCookie:
                            HttpContextAccessor.HttpContext?.Response.Cookies.Delete("Session");
                            break;
                        default:
                            Debug.WriteLine(ex.Message);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

    }
}