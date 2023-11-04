using Firebase.Auth;
using FirebaseAdmin.Auth;
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
            await CreateCookieAsync(userCredential.User.Credential.IdToken);
            return userCredential;
        }

        public async Task<Firebase.Auth.UserCredential> LoginAsync(string email, string password)
        {
            Firebase.Auth.UserCredential userCredential = await FirebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            AddUserDetailsToSession(userCredential.User.Uid, userCredential.User.Info.Email, userCredential.User.Info.DisplayName);
            await CreateCookieAsync(userCredential.User.Credential.IdToken);
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

        public async Task CreateCookieAsync(string idToken)
        {
            SessionCookieOptions sessionCookieOptions = new SessionCookieOptions
            {
                ExpiresIn = TimeSpan.FromDays(14)
            };

            string sessionCookie = await FirebaseAuth.CreateSessionCookieAsync(idToken, sessionCookieOptions);

            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.Add(sessionCookieOptions.ExpiresIn),
                HttpOnly = true,
                Secure = true
            };

            HttpContextAccessor.HttpContext?.Response.Cookies.Append("Session", sessionCookie, cookieOptions);
        }

        public async Task<FirebaseToken> VerifyCookieAsync(string sessionCookie)
        {
            FirebaseToken firebaseToken = await FirebaseAuth.VerifySessionCookieAsync(sessionCookie);
            return firebaseToken;
        }

        public async Task Logout()
        {
            string? uid = HttpContextAccessor.HttpContext?.Session.GetString("UserId");
            try
            {
                await FirebaseAuth.RevokeRefreshTokensAsync(uid);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            HttpContextAccessor.HttpContext?.Session.Clear();
            HttpContextAccessor.HttpContext?.Response.Cookies.Delete("Session");
        }
    
        //helper methods
        private void AddUserDetailsToSession(string uid, string email, string displayName)
        {
            HttpContextAccessor.HttpContext?.Session.SetString("UserId", uid);
            HttpContextAccessor.HttpContext?.Session.SetString("Email", email);
            HttpContextAccessor.HttpContext?.Session.SetString("DisplayName", displayName);
        }

    }
}
