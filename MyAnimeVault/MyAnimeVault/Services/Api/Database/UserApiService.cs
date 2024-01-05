using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Services.Api.Database;
using Newtonsoft.Json;

namespace MyAnimeVault.Services.Api.Database
{
    public class UserApiService : IUserApiService
    {
        private readonly IHttpClientFactory HttpClientFactory;
        private readonly HttpClient HttpClient;
        private readonly string ApiKey;

        public UserApiService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            ApiKey = config.GetValue<string>("MyAnimeVaultApiKey");
            HttpClientFactory = httpClientFactory;
            HttpClient = HttpClientFactory.CreateClient();
            HttpClient.BaseAddress = new Uri("https://myanimevaultapi.azurewebsites.net/");
            HttpClient.DefaultRequestHeaders.Add("x-api-key", ApiKey);
        }
        public Task<bool> AddAnimeToList(int userId, UserAnime userAnime)
        {
            throw new NotImplementedException();
        }

        public Task<User> AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"api/users/{id}");
            if(response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                User? user = JsonConvert.DeserializeObject<User>(jsonString);
                return user;
            }

            return null;
        }

        public Task<User> GetUserByUid(int uid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAnimeFromList(int userId, UserAnime userAnime)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
