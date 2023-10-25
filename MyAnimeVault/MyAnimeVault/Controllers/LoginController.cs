using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Services.Authentication;

namespace MyAnimeVault.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController > _logger;
        private readonly IAuthenticator Authenticator;

        public LoginController(ILogger<LoginController> logger, IAuthenticator authenticator)
        {
            _logger = logger;
            Authenticator = authenticator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
    }
}
