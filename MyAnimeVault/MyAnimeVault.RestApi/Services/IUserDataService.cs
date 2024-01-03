using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework.Services;
using MyAnimeVault.RestApi.Models.DTOs;

namespace MyAnimeVault.RestApi.Services
{
    public interface IUserDataService : IGenericDataService<User>
    {
        Task<User?> GetByUidAsync(string uid);
        Task<UserDTO?> GetByUidAsDTOAsync(string uid);
        Task<UserDTO?> GetByIdAsDTOAsync(int id);
        Task<List<UserDTO>?> GetAllAsDTOsAsync();
        Task<UserDTO> AddAndReturnDTOAsync(User user);
        Task<UserDTO> UpdateAndReturnDTOAsync(User user);
        Task<bool> AddAnimeToListAsync(User user, UserAnime anime);
        Task<bool> RemoveAnimeFromListAsync(User user, UserAnime anime);
    }
}
