using MyAnimeVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Services.Api.Database
{
    public interface IUserApiService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User> GetUserByUid(int uid);
        Task<List<User>>GetAllUsers();
        Task<User> AddUser(User user);
        Task<bool> AddAnimeToList(int userId, UserAnime userAnime);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<bool> RemoveAnimeFromList(int userId, UserAnime userAnime);

    }
}
