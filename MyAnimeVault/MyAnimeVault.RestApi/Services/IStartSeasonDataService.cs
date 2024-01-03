using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework.Services;
using MyAnimeVault.RestApi.Models.DTOs;

namespace MyAnimeVault.RestApi.Services
{
    public interface IStartSeasonDataService : IGenericDataService<StartSeason>
    {
        Task<StartSeasonDTO?> GetByIdAsDTOAsync(int id);
        Task<List<StartSeasonDTO>?> GetAllAsDTOsAsync();
        Task<StartSeasonDTO?> AddAndReturnDTOAsync(StartSeason entity);
        Task<StartSeasonDTO?> UpdateAndReturnDTOAsync(StartSeason entity);
    }
}
