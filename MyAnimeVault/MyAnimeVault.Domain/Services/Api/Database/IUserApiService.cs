using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Services.Api.Database
{
    public interface IUserApiService
    {
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO?> GetUserByUidAsync(string uid);
        Task<List<UserDTO>?>GetAllUsersAsync();
        Task<UserDTO?> AddUserAsync(UserDTO userDTO);
        Task<bool> AddAnimeToListAsync(int userId, UserAnimeDTO userAnimeDTO);
        Task<UserDTO?> UpdateUserAsync(UserDTO userDTO);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> RemoveAnimeFromListAsync(int userId, int userAnimeId);

    }
}
