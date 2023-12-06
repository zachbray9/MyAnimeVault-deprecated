using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Services;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.EntityFramework.Services;
using MyAnimeVault.Models;
using MyAnimeVault.MyAnimeListApi.Models;
using MyAnimeVault.Services.Authentication;
using System.Diagnostics;

namespace MyAnimeVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly MyAnimeVaultDbContext DbContext;
        private readonly IAuthenticator Authenticator;
        private readonly IUserDataService UserDataService;
        private readonly IGenericDataService<UserAnime> UserAnimeDataService;
        private readonly IGenericDataService<Poster> PosterDataService;
        private readonly IGenericDataService<StartSeason> StartSeasonDataService; 
        private readonly IAnimeApiService AnimeApiService;

        public List<AnimeListNode> AnimeList { get; set; } = new List<AnimeListNode>();

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, MyAnimeVaultDbContext dbContext, IAuthenticator authenticator, IUserDataService userDataService, IGenericDataService<UserAnime> userAnimeDataService, IGenericDataService<Poster> posterDataService, IGenericDataService<StartSeason> startSeasonDataService, IAnimeApiService animeApiService)
        {
            _logger = logger;
            HttpContextAccessor = httpContextAccessor;
            DbContext = dbContext;
            Authenticator = authenticator;
            UserDataService = userDataService;
            UserAnimeDataService = userAnimeDataService;
            PosterDataService = posterDataService;
            StartSeasonDataService = startSeasonDataService;
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

            try
            {
                User? user = await UserDataService.GetByUidAsync(uid);

                if(user != null && !user.Animes.Any(userAnime =>  userAnime.Id == id))
                {
                    Anime anime = await AnimeApiService.GetAnimeById(id);
                    Poster? existingPoster = null;
                    StartSeason? existingStartSeason = null;

                    //checks if the anime has a poster and, if so, checks if it's already in the database. If not, then add it to the database.
                    if(anime.Picture != null)
                    {
                        existingPoster = await DbContext.Posters.FirstOrDefaultAsync(p => p.Medium == anime.Picture.Medium);
                        if(existingPoster == null)
                        {
                            existingPoster = await PosterDataService.AddAsync(anime.Picture);
                        }
                    }

                    //checks if the anime has a start season and, if so, checks if it's already in the database. If not, then add it to the database.
                    if(anime.StartSeason != null)
                    {
                        existingStartSeason = await DbContext.StartSeasons.FirstOrDefaultAsync(ss => ss.Season == anime.StartSeason.Season && ss.Year == anime.StartSeason.Year);
                        if(existingStartSeason == null)
                        {
                            existingStartSeason = await StartSeasonDataService.AddAsync(anime.StartSeason);
                        }
                    }

                    UserAnime animeToAdd = new UserAnime
                    {
                        AnimeId = id,
                        UserId = user.Id,
                        Title = (anime.AlternativeTitles != null && !string.IsNullOrEmpty(anime.AlternativeTitles.en)) ? anime.AlternativeTitles.en : anime.Title,
                        PosterId = existingPoster != null ? existingPoster.Id : null,
                        StartSeasonId = existingStartSeason != null ? existingStartSeason.Id : null,
                        MediaType = anime.MediaType,
                        TotalEpisodes = anime.NumEpisodes,
                        Status = anime.Status
                    };

                    await UserDataService.AddAnimeToList(user, animeToAdd);
                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
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