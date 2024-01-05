using MyAnimeVault.Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Services.Api.Database
{
    public interface IUserAnimeApiService
    {
        Task<UserAnimeDTO?> GetUserAnimeByIdAsync(int id);
        Task<List<UserAnimeDTO>?> GetAllUserAnimesAsync();
        Task<UserAnimeDTO?> AddUserAnimeAsync(UserAnimeDTO useranimeDTO);
        Task<UserAnimeDTO?> UpdateUserAnimeAsync(UserAnimeDTO useranimeDTO);
        Task<bool> DeleteUserAnimeAsync(int id);
    }
}
