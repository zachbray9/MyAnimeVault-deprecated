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
        private readonly IGenericDataService<UserAnime> UserAnimeDataService;
        private readonly IAnimeApiService AnimeApiService;

        public List<AnimeListNode> AnimeList { get; set; } = new List<AnimeListNode>();

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IAuthenticator authenticator, IUserDataService userDataService, IGenericDataService<UserAnime> userAnimeDataService, IAnimeApiService animeApiService)
        {
            _logger = logger;
            HttpContextAccessor = httpContextAccessor;
            Authenticator = authenticator;
            UserDataService = userDataService;
            UserAnimeDataService = userAnimeDataService;
            AnimeApiService = animeApiService;
        }

        public async Task<IActionResult> Index()
        {
            await ValidateUserSession();
            AnimeList = await AnimeApiService.GetAllAnime();
            return View(AnimeList);
        }

        public async Task<IActionResult> AnimeDetails(int id) 
        {
            await ValidateUserSession();

            AnimeDetailsViewModel viewModel = new AnimeDetailsViewModel();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            viewModel.Anime = await AnimeApiService.GetAnimeById(id);

            if(uid != null)             //checks if there is a user logged in 
            {
                viewModel.CurrentUser = await UserDataService.GetByUidAsync(uid);
                if(viewModel.CurrentUser != null) 
                {
                    viewModel.AnimeIsOnUsersList = viewModel.CurrentUser.Animes.Any(a => a.AnimeId == viewModel.Anime.Id) ? true : false;
                }
            }


            return View(viewModel);
        }

        public async Task<IActionResult> SearchResults(string query)
        {
            await ValidateUserSession();
            List<AnimeListNode> SearchResults = await AnimeApiService.GetListOfAnimeByQuery(query);
            return View(SearchResults);
        }

        public async Task<IActionResult> Vault() 
        {
            await ValidateUserSession();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            if(uid == null)
            {
                return RedirectToAction("Index", "Login"); //if user is not logged in, redirect to login page
            }

            User? currentUser = await UserDataService.GetByUidAsync(uid); //if user is logged in, retrieve user from database and pass their anime list into the vault view
            return View(currentUser?.Animes);

        }

        public async Task<IActionResult> AddAnimeToUserList(int id)
        {
            await ValidateUserSession();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (uid == null)
            {
                return RedirectToAction("Index", "Login");
            }

            User? user = await UserDataService.GetByUidAsync(uid);

            if(user != null && !user.Animes.Any(userAnime =>  userAnime.Id == id))
            {
                //add new UserAnime to users list
            }

            return RedirectToAction("AnimeDetails", "Home", new { id = id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //helper methods
        private async Task ValidateUserSession() //checks if the session cookie is still valid and, if so, populates the session variables with uid, email, and display name. Otherwise does nothing
        {
            string? sessionCookie = HttpContextAccessor.HttpContext?.Request.Cookies["Session"];
        
            if (!string.IsNullOrEmpty(sessionCookie)) //checks if there is a session cookie
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