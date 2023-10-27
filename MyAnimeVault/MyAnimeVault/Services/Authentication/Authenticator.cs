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

        public async Task<UserCredential> RegisterAsync(string email, string password, string displayName)
        {
            UserCredential userCredential = await FirebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
            return userCredential;
        }

        public async Task<UserCredential> LoginAsync(string email, string password)
        {
            UserCredential userCredential = await FirebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            return userCredential;
        }

    }
}
