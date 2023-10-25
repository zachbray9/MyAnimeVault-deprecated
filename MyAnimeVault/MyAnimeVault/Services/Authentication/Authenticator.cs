using Firebase.Auth;

namespace MyAnimeVault.Services.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly FirebaseAuthClient FirebaseAuthClient;

        public Authenticator(FirebaseAuthClient firebaseAuthClient)
        {
            FirebaseAuthClient = firebaseAuthClient;
        }

        public async Task<UserCredential> RegisterAsync(string email, string displayName, string password)
        {
            UserCredential userCredential = await FirebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, displayName, password);
            return userCredential;
        }

        public async Task<UserCredential> LoginAsync(string email, string password)
        {
            UserCredential userCredential = await FirebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            return userCredential;
        }

    }
}
