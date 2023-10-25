using Firebase.Auth;

namespace MyAnimeVault.Services.Authentication
{
    public interface IAuthenticator
    {
        Task<UserCredential> RegisterAsync(string email, string password, string confirmPassword, string displayName);
        Task<UserCredential> LoginAsync(string email, string password);
    }
}
