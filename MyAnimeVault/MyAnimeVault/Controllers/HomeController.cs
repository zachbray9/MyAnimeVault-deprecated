using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Services;
using MyAnimeVault.EntityFramework.Services;
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
        private readonly IUserDataService UserDataService;
        private readonly IAnimeApiService AnimeApiService;

        public List<AnimeListNode> AnimeList { get; set; } = new List<AnimeListNode>();

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IAuthenticator authenticator, IUserDataService userDataService, IAnimeApiService animeApiService)
        {
            _logger = logger;
            HttpContextAccessor = httpContextAccessor;
            Authenticator = authenticator;
            UserDataService = userDataService;
            AnimeApiService = animeApiService;
        }

        public async Task<IActionResult> Index()
        {
            await StoreUserDataInSession();
            AnimeList = await AnimeApiService.GetAllAnime();
            return View(AnimeList);
        }

        public async Task<IActionResult> AnimeDetails(int id) 
        {
            await StoreUserDataInSession();

            AnimeDetailsViewModel viewModel = new AnimeDetailsViewModel();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            viewModel.Anime = await AnimeApiService.GetAnimeById(id);
            if(uid != null)
            {
                viewModel.CurrentUser = await UserDataService.GetByUidAsync(uid);
            }


            return View(viewModel);
        }

        public async Task<IActionResult> SearchResults(string query)
        {
            await StoreUserDataInSession();
            List<AnimeListNode> SearchResults = await AnimeApiService.GetListOfAnimeByQuery(query);
            return View(SearchResults);
        }

        public async Task<IActionResult> Vault() 
        {
            await StoreUserDataInSession();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            if(uid == null)
            {
                return RedirectToAction("Index", "Login"); //if user is not logged in, redirect to login page
            }

            User? currentUser = await UserDataService.GetByUidAsync(uid); //if user is logged in, retrieve user from database and pass their anime list into the vault view
            return View(currentUser?.Animes);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //helper methods
        private async Task StoreUserDataInSession() //checks if the user is already logged in or has a session cookie. If not, user must login
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