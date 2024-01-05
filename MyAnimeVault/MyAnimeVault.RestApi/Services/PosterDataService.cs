using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.Domain.Models.DTOs;

namespace MyAnimeVault.RestApi.Services
{
    public class PosterDataService : IPosterDataService
    {
        private readonly MyAnimeVaultDbContext DbContext;

        public PosterDataService(MyAnimeVaultDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<PosterDTO?> AddAndReturnDTOAsync(Poster entity)
        {
            EntityEntry<Poster> createdResult = await DbContext.Set<Poster>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            PosterDTO posterDTO = MapToDTO(createdResult.Entity);
            return posterDTO;
        }

        public async Task<Poster> AddAsync(Poster entity)
        {
            EntityEntry<Poster> createdResult = await DbContext.Set<Poster>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Poster? entity = await DbContext.Set<Poster>().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return false;
            }

            DbContext.Set<Poster>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<PosterDTO>?> GetAllAsDTOsAsync()
        {
            List<PosterDTO> posterDTOs = MapToListOfDTOs(await DbContext.Posters.ToListAsync());
            return posterDTOs;
        }

        public async Task<List<Poster>?> GetAllAsync()
        {
            List<Poster>? posters = await DbContext.Posters.ToListAsync();
            return posters;
        }

        public async Task<PosterDTO?> GetByIdAsDTOAsync(int id)
        {
            Poster? poster = await DbContext.Posters.FirstOrDefaultAsync(u => u.Id == id);
            if (poster != null)
            {
                PosterDTO posterDTO = MapToDTO(poster);
                return posterDTO;
            }
            return null;
        }

        public async Task<Poster?> GetByIdAsync(int id)
        {
            Poster? poster = await DbContext.Posters.FirstOrDefaultAsync(u => u.Id == id);
            return poster;
        }

        public async Task<PosterDTO?> UpdateAndReturnDTOAsync(Poster entity)
        {
            EntityEntry<Poster> result = DbContext.Set<Poster>().Update(entity);
            await DbContext.SaveChangesAsync();
            PosterDTO posterDTO = MapToDTO(result.Entity);
            return posterDTO;
        }

        public async Task<Poster> UpdateAsync(Poster entity)
        {
            EntityEntry<Poster> result = DbContext.Set<Poster>().Update(entity);
            await DbContext.SaveChangesAsync();
            return result.Entity;
        }

        //Map to DTO method
        private PosterDTO MapToDTO(Poster userEntity)
        {
            return new PosterDTO
            {
                Id = userEntity.Id,
                Large = userEntity.Large,
                Medium = userEntity.Medium
            };
        }

        private List<PosterDTO> MapToListOfDTOs(List<Poster> userEntities)
        {
            List<PosterDTO> userAnimeDTOs = userEntities.Select(ua => new PosterDTO
            {
                Id = ua.Id,
                Large = ua.Large,
                Medium = ua.Medium
            }).ToList();

            return userAnimeDTOs;
        }
    }
}
