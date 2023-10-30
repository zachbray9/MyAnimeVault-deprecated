using Firebase.Auth;
using FirebaseAdmin.Auth;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MyAnimeVault.Services.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly FirebaseAuthClient FirebaseAuthClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public Authenticator(FirebaseAuthClient firebaseAuthClient, IHttpContextAccessor httpContextAccessor)
        {
            FirebaseAuthClient = firebaseAuthClient;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<UserCredential> RegisterAsync(string email, string password, string displayName)
        {
            UserCredential userCredential = await FirebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
            AddUserDetailsToSession(userCredential);
            return userCredential;
        }

        public async Task<UserCredential> LoginAsync(string email, string password)
        {
            UserCredential userCredential = await FirebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            AddUserDetailsToSession(userCredential);
            return userCredential;
        }

        public async Task<UserCredential> LoginWithCredentialAsync(AuthCredential authCredential)
        {
            UserCredential userCredential = await FirebaseAuthClient.SignInWithCredentialAsync(authCredential);
            AddUserDetailsToSession(userCredential);
            return userCredential;
        }

        public async Task<FirebaseToken> VerifyIdToken(string IdToken)
        {
            
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }
    
        //helper methods
        private void AddUserDetailsToSession(UserCredential userCredential)
        {
            HttpContextAccessor.HttpContext.Session.SetString("UserId", userCredential.User.Uid);
            HttpContextAccessor.HttpContext.Session.SetString("Email", userCredential.User.Info.Email);
            HttpContextAccessor.HttpContext.Session.SetString("DisplayName", userCredential.User.Info.DisplayName);
            HttpContextAccessor.HttpContext.Session.SetString("FirebaseToken", userCredential.User.Credential.IdToken);

            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddSeconds(userCredential.User.Credential.ExpiresIn)
            };

            HttpContextAccessor.HttpContext.Response.Cookies.Append("FirebaseToken", userCredential.User.Credential.IdToken, cookieOptions);
        }

    }
}
