using Firebase.Auth;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.Domain.Services.Api.Database;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<bool> AddAnimeToListAsync(int userId, UserAnimeDTO userAnimeDTO)
        {
            var jsonString = JsonConvert.SerializeObject(userAnimeDTO);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PostAsync($"api/users/addanimetolist/{userId}", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<UserDTO?> AddUserAsync(UserDTO userDTO)
        {
            string jsonString = JsonConvert.SerializeObject(userDTO);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PostAsync($"api/users", content);
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
                UserDTO? returnedUser = JsonConvert.DeserializeObject<UserDTO>(jsonString);
                return returnedUser;
            }

            return null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"api/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<List<UserDTO>?> GetAllUsersAsync()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"api/users");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                List<UserDTO>? users = JsonConvert.DeserializeObject<List<UserDTO>?>(jsonString);
                return users;
            }

            return null;
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"api/users/{id}");
            if(response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                UserDTO? user = JsonConvert.DeserializeObject<UserDTO>(jsonString);
                return user;
            }

            return null;
        }

        public async Task<UserDTO?> GetUserByUidAsync(string uid)
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"api/users/{uid}");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                UserDTO? user = JsonConvert.DeserializeObject<UserDTO>(jsonString);
                return user;
            }

            return null;
        }

        public async Task<bool> RemoveAnimeFromListAsync(int userId, int userAnimeId)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"api/users/RemoveAnimeFromList/{userId}/{userAnimeId}");
            if(response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<UserDTO?> UpdateUserAsync(UserDTO userDTO)
        {
            string jsonString = JsonConvert.SerializeObject(userDTO);
            StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PutAsync($"api/users", content);
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
                UserDTO? updatedUserDTO = JsonConvert.DeserializeObject<UserDTO>(jsonString);
                return updatedUserDTO;
            }

            return null;
        }
    }
}
