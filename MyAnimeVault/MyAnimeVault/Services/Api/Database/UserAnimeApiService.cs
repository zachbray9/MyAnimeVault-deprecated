using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.Domain.Services.Api.Database;
using Newtonsoft.Json;
using System.Text;

namespace MyAnimeVault.Services.Api.Database
{
    public class UserAnimeApiService : IUserAnimeApiService
    {
        private readonly IHttpClientFactory HttpClientFactory;
        private readonly HttpClient HttpClient;
        private readonly string ApiKey;

        public UserAnimeApiService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            ApiKey = config.GetValue<string>("MyAnimeVaultApiKey");
            HttpClientFactory = httpClientFactory;
            HttpClient = HttpClientFactory.CreateClient();
            HttpClient.BaseAddress = new Uri("https://myanimevaultapi.azurewebsites.net/");
            HttpClient.DefaultRequestHeaders.Add("x-api-key", ApiKey);
        }

        public async Task<UserAnimeDTO?> AddUserAnimeAsync(UserAnimeDTO useranimeDTO)
        {
            string jsonString = JsonConvert.SerializeObject(useranimeDTO);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PostAsync($"api/useranimes", content);
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
                UserAnimeDTO? returnedUserAnime = JsonConvert.DeserializeObject<UserAnimeDTO>(jsonString);
                return returnedUserAnime;
            }

            return null;
        }

        public async Task<bool> DeleteUserAnimeAsync(int id)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"api/useranimes/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<List<UserAnimeDTO>?> GetAllUserAnimesAsync()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"api/useranimes");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                List<UserAnimeDTO>? userAnimes = JsonConvert.DeserializeObject<List<UserAnimeDTO>?>(jsonString);
                return userAnimes;
            }

            return null;
        }

        public async Task<UserAnimeDTO?> GetUserAnimeByIdAsync(int id)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"api/useranimes/{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                UserAnimeDTO? userAnimeDTO = JsonConvert.DeserializeObject<UserAnimeDTO>(jsonString);
                return userAnimeDTO;
            }

            return null;
        }

        public async Task<UserAnimeDTO?> UpdateUserAnimeAsync(UserAnimeDTO userAnimeDTO)
        {
            string jsonString = JsonConvert.SerializeObject(userAnimeDTO);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PutAsync($"api/useranimes", content);
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
                UserAnimeDTO? updatedUserAnimeDTO = JsonConvert.DeserializeObject<UserAnimeDTO>(jsonString);
                return updatedUserAnimeDTO;
            }

            return null;
        }
    }
}
