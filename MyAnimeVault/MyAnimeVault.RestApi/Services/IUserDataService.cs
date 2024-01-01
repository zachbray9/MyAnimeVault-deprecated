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
        Task<bool> AddAnimeToList(User user, UserAnime anime);
        Task<bool> RemoveAnimeFromList(User user, UserAnime anime);
    }
}
