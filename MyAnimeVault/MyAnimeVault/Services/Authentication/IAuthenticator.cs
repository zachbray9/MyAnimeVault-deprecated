using Firebase.Auth;
using FirebaseAdmin.Auth;

namespace MyAnimeVault.Services.Authentication
{
    public interface IAuthenticator
    {
        Task<UserCredential> RegisterAsync(string email, string password, string displayName);
        Task<UserCredential> LoginAsync(string email, string password);
        Task<UserCredential> LoginWithCredentialAsync(AuthCredential authCredential);
        Task<FirebaseToken> VerifyIdToken(string IdToken);
        Task Logout();
    }
}
