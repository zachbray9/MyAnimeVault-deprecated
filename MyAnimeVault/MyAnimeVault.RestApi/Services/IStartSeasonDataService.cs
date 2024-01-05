using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.EntityFramework.Services;

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
