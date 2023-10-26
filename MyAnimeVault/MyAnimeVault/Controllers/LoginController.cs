using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Models;
using MyAnimeVault.Services.Authentication;
using System.Diagnostics;

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
            CreateAccountViewModel viewModel = new CreateAccountViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                //attempt to create accounnt
                try
                {
                    //store current user and redirect to home page
                    UserCredential userCredential = await Authenticator.RegisterAsync(viewModel.Email, viewModel.DisplayName, viewModel.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch(FirebaseAuthException ex)
                {
                    switch(ex.Reason)
                    {
                        case AuthErrorReason.EmailExists:
                            ModelState.AddModelError(string.Empty, "The email you are trying to use already exists.");
                            break;
                        case AuthErrorReason.WeakPassword:
                            ModelState.AddModelError(string.Empty, "Password must be more than 6 characters.");
                            break;
                        default:
                            ModelState.AddModelError(string.Empty, "An error occurred during account creation. Please try again.");
                            break;
                    }
                }
            }

            //If there is an error on the form then return to the create account page with error message
            return View(viewModel);
        }
    }
}
