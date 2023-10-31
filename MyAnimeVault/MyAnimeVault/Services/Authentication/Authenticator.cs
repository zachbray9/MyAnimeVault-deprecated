using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Requests;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace MyAnimeVault.Services.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly FirebaseAuthClient FirebaseAuthClient;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly FirebaseAuth FirebaseAuth;

        public Authenticator(FirebaseAuthClient firebaseAuthClient, IHttpContextAccessor httpContextAccessor, FirebaseAuth firebaseAuth)
        {
            FirebaseAuthClient = firebaseAuthClient;
            HttpContextAccessor = httpContextAccessor;
            FirebaseAuth = firebaseAuth;
        }

        public async Task<Firebase.Auth.UserCredential> RegisterAsync(string email, string password, string displayName)
        {
            Firebase.Auth.UserCredential userCredential = await FirebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
            AddUserDetailsToSession(userCredential.User.Uid, userCredential.User.Info.Email, userCredential.User.Info.DisplayName);
            CreateCookie(userCredential);
            return userCredential;
        }

        public async Task<Firebase.Auth.UserCredential> LoginAsync(string email, string password)
        {
            Firebase.Auth.UserCredential userCredential = await FirebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            AddUserDetailsToSession(userCredential.User.Uid, userCredential.User.Info.Email, userCredential.User.Info.DisplayName);
            CreateCookie(userCredential);
            return userCredential;
        }

        public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.VerifyIdTokenAsync(idToken);
            return firebaseToken;
        }

        public async Task<UserRecord> GetUserByUidAsync(string uid)
        {
            UserRecord userRecord = await FirebaseAuth.GetUserAsync(uid);
            AddUserDetailsToSession(userRecord.Uid, userRecord.Email, userRecord.DisplayName);
            return userRecord;
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }
    
        //helper methods
        private void AddUserDetailsToSession(string uid, string email, string displayName)
        {
            HttpContextAccessor.HttpContext.Session.SetString("UserId", uid);
            HttpContextAccessor.HttpContext.Session.SetString("Email", email);
            HttpContextAccessor.HttpContext.Session.SetString("DisplayName", displayName);
        }

        private void CreateCookie(Firebase.Auth.UserCredential userCredential)
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddSeconds(userCredential.User.Credential.ExpiresIn)
            };

            HttpContextAccessor.HttpContext.Response.Cookies.Append("FirebaseToken", userCredential.User.Credential.IdToken, cookieOptions);
            HttpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", userCredential.User.Credential.RefreshToken, cookieOptions);
        }

    }
}
