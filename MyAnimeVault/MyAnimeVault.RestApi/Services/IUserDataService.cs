using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.EntityFramework.Services;

namespace MyAnimeVault.RestApi.Services
{
    public interface IUserDataService : IGenericDataService<User>
    {
        Task<User?> GetByUidAsync(string uid);
        Task<UserDTO?> GetByUidAsDTOAsync(string uid);
        Task<UserDTO?> GetByIdAsDTOAsync(int id);
        Task<List<UserDTO>?> GetAllAsDTOsAsync();
        Task<UserDTO?> AddAndReturnDTOAsync(UserDTO newUser);
        Task<UserDTO?> UpdateAndReturnDTOAsync(UserDTO user);
        Task<bool> AddAnimeToListAsync(int userId, UserAnimeDTO userAnimeDTO);
        Task<bool> RemoveAnimeFromListAsync(int userId, int userAnimeId);
    }
}
