using Firebase.Auth;
using FirebaseAdmin.Auth;

namespace MyAnimeVault.Services.Authentication
{
    public interface IAuthenticator
    {
        Task<UserCredential> RegisterAsync(string email, string password, string displayName);
        Task<UserCredential> LoginAsync(string email, string password);
        Task<FirebaseToken> VerifyIdTokenAsync(string idToken);
        Task<UserRecord> GetUserByUidAsync(string uid);
        Task CreateCookieAsync(string idToken);
        Task<FirebaseToken> VerifyCookieAsync(string sessionCookie);
        Task Logout();
    }
}
