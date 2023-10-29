using Firebase.Auth;

namespace MyAnimeVault.Services.Authentication
{
    public interface IAuthenticator
    {
        Task<UserCredential> RegisterAsync(string email, string password, string displayName);
        Task<UserCredential> LoginAsync(string email, string password);
        Task Logout();
    }
}
