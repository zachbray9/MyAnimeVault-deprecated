using Firebase.Auth;

namespace MyAnimeVault.Services.Authentication
{
    public interface IAuthenticator
    {
        Task<UserCredential> RegisterAsync(string email, string displayName, string password);
        Task<UserCredential> LoginAsync(string email, string password);
    }
}
