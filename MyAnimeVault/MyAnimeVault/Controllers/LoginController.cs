using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Models;
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
            LoginViewModel viewModel = new LoginViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                //attempt to login
                try
                {
                    //store current user and direct to homepage
                    UserCredential userCredential = await Authenticator.LoginAsync(viewModel.Email, viewModel.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch(FirebaseAuthException ex)
                {
                    switch(ex.Reason)
                    {
                        case AuthErrorReason.TooManyAttemptsTryLater:
                            ModelState.AddModelError(string.Empty, "Too many login attempts have been made. Try again later.");
                            break;
                        case AuthErrorReason.UnknownEmailAddress:
                            ModelState.AddModelError(string.Empty, "No account with this email exists. Please try a different email.");
                            break;
                        case AuthErrorReason.WrongPassword:
                            ModelState.AddModelError(string.Empty, "The supplied password is not valid for this email address.");
                            break;
                        case AuthErrorReason.UserDisabled:
                            ModelState.AddModelError(string.Empty, "This user was disabled and not granted access anymore.");
                            break;
                        case AuthErrorReason.UserNotFound:
                            ModelState.AddModelError(string.Empty, "The user account does not exist. Please check the entered information or create a new account if you are a new user.");
                            break;
                        default:
                            ModelState.AddModelError(string.Empty, "An error occurred during the login attempt. Please check the entered information oro create a new account if you are a new user.");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            //if there is an error on the form then return to login page and display error
            return View(viewModel);
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
                    UserCredential userCredential = await Authenticator.RegisterAsync(viewModel.Email, viewModel.Password, viewModel.DisplayName);
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
