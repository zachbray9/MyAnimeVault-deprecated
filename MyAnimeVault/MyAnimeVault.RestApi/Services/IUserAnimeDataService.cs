using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework.Services;
using MyAnimeVault.RestApi.Models.DTOs;

namespace MyAnimeVault.RestApi.Services
{
    public interface IUserAnimeDataService : IGenericDataService<UserAnime>
    {
        Task<UserAnimeDTO?> GetByIdAsDTOAsync(int id);
        Task<List<UserAnimeDTO>?> GetAllAsDTOsAsync();
        Task<UserAnimeDTO?> AddAndReturnDTOAsync(UserAnime entity); 
        Task<UserAnimeDTO?> UpdateAndReturnDTOAsync(UserAnime entity);
    }
}
