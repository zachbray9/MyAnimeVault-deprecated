using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.Domain.Services;
using MyAnimeVault.Domain.Services.Api.Database;
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
        private readonly IUserApiService UserApiService;
        private readonly IUserAnimeApiService UserAnimeApiService;
        private readonly IAnimeApiService AnimeApiService;

        public List<AnimeListNode> AnimeList { get; set; } = new List<AnimeListNode>();

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IAuthenticator authenticator, IUserApiService userApiService, IUserAnimeApiService userAnimeApiService, IAnimeApiService animeApiService)
        {
            _logger = logger;
            HttpContextAccessor = httpContextAccessor;
            Authenticator = authenticator;
            UserApiService = userApiService;
            UserAnimeApiService = userAnimeApiService;
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
                viewModel.CurrentUser = await UserApiService.GetUserByUidAsync(uid);
                if(viewModel.CurrentUser != null) 
                {
                    //checks if the specific anime is already on the users list or not
                    viewModel.UserAnime = viewModel.CurrentUser.Animes.FirstOrDefault(a => a.AnimeId == viewModel.Anime.Id);
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

            UserDTO? currentUser = await UserApiService.GetUserByUidAsync(uid); //if user is logged in, retrieve user from database and pass their anime list into the vault view
            return View(currentUser?.Animes);
        }

        public async Task<IActionResult> AddAnimeToUserList(int userId, int animeId)
        {
            await ValidateUserSession();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (uid == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Anime anime = await AnimeApiService.GetAnimeById(animeId);

            UserAnimeDTO userAnimeDTO = new UserAnimeDTO
            {
                AnimeId = animeId,
                Title = (anime.AlternativeTitles != null && !string.IsNullOrEmpty(anime.AlternativeTitles.en)) ? anime.AlternativeTitles.en : anime.Title,
                MediaType = anime.MediaType,
                TotalEpisodes = anime.NumEpisodes,
                Status = anime.Status,
                Poster = anime.Picture != null ? new PosterDTO
                {
                    Large = anime.Picture.Large,
                    Medium = anime.Picture.Medium
                } : null,
                StartSeason = anime.StartSeason != null ? new StartSeasonDTO
                {
                    Year = anime.StartSeason.Year,
                    Season = anime.StartSeason.Season,
                } : null
            };

            await UserApiService.AddAnimeToListAsync(userId, userAnimeDTO);

            return RedirectToAction("AnimeDetails", "Home", new { id = animeId });
        }

        public async Task<IActionResult> RemoveAnimeFromUserList(int animeId, int userAnimeId)
        {
            await ValidateUserSession();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (uid == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                UserDTO? user = await UserApiService.GetUserByUidAsync(uid);

                if(user != null && user.Animes.Any(ua => ua.AnimeId == animeId))
                {
                    UserAnimeDTO? userAnime = await UserAnimeApiService.GetUserAnimeByIdAsync(userAnimeId);
                    if(userAnime != null)
                    {
                        await UserApiService.RemoveAnimeFromListAsync(user.Id, userAnime.Id);
                    }
                }
                    
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return RedirectToAction("AnimeDetails", "Home", new { id = animeId });
        }



        [HttpPost]
        public async Task<IActionResult> UpdateUserAnimeWatchStatus(int userId, int animeId, string value)
        {
            await ValidateUserSession();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (uid == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                UserDTO? user = await UserApiService.GetUserByIdAsync(userId);
                if (user != null)
                {
                    UserAnimeDTO? userAnime = user.Animes.FirstOrDefault(ua => ua.AnimeId == animeId);
                    if (userAnime != null)
                    {
                        userAnime.WatchStatus = value;
                        userAnime = await UserAnimeApiService.UpdateUserAnimeAsync(userAnime);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }



        [HttpPost]
        public async Task<IActionResult> UpdateUserAnimeRatingOrEpisodesWatched(int userId, int animeId, string fieldName, int value)
        {
            await ValidateUserSession();
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");

            if (uid == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                UserDTO? user = await UserApiService.GetUserByIdAsync(userId);
                if (user != null)
                {
                    UserAnimeDTO? userAnime = user.Animes.FirstOrDefault(ua => ua.AnimeId == animeId);
                    if (userAnime != null)
                    {
                        switch (fieldName)
                        {
                            case "rating":
                                userAnime.Rating = value;
                                break;
                            case "episodesWatched":
                                if(!(value > 0 && value <= userAnime.TotalEpisodes))
                                {
                                    return Json(new { success = false });
                                }
                                userAnime.NumEpisodesWatched = value;
                                break;
                            default:
                                return Json(new { success = false });
                        }
                        userAnime = await UserAnimeApiService.UpdateUserAnimeAsync(userAnime);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Json(new { success = false });
            }

            return Json(new { success = true });
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