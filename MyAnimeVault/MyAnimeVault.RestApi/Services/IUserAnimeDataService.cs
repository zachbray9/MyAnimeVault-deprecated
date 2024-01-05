using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.EntityFramework.Services;

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
