using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.EntityFramework.Services;

namespace MyAnimeVault.RestApi.Services
{
    public interface IPosterDataService : IGenericDataService<Poster>
    {
        Task<PosterDTO?> GetByIdAsDTOAsync(int id);
        Task<List<PosterDTO>?> GetAllAsDTOsAsync();
        Task<PosterDTO?> AddAndReturnDTOAsync(Poster entity);
        Task<PosterDTO?> UpdateAndReturnDTOAsync(Poster entity);
    }
}
