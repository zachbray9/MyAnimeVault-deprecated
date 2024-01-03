using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.RestApi.Models.DTOs;

namespace MyAnimeVault.RestApi.Services
{
    public class StartSeasonDataService : IStartSeasonDataService
    {
        private readonly MyAnimeVaultDbContext DbContext;

        public StartSeasonDataService(MyAnimeVaultDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<StartSeasonDTO?> AddAndReturnDTOAsync(StartSeason entity)
        {
            EntityEntry<StartSeason> createdResult = await DbContext.Set<StartSeason>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            StartSeasonDTO startSeasonDTO = MapToDTO(createdResult.Entity);
            return startSeasonDTO;
        }

        public async Task<StartSeason> AddAsync(StartSeason entity)
        {
            EntityEntry<StartSeason> createdResult = await DbContext.Set<StartSeason>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            StartSeason? entity = await DbContext.Set<StartSeason>().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return false;
            }

            DbContext.Set<StartSeason>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<StartSeasonDTO>?> GetAllAsDTOsAsync()
        {
            List<StartSeasonDTO> startSeasonDTOs = MapToListOfDTOs(await DbContext.StartSeasons.ToListAsync());
            return startSeasonDTOs;
        }

        public async Task<List<StartSeason>?> GetAllAsync()
        {
            List<StartSeason>? startSeasons = await DbContext.StartSeasons.ToListAsync();
            return startSeasons;
        }

        public async Task<StartSeasonDTO?> GetByIdAsDTOAsync(int id)
        {
            StartSeason? startSeason = await DbContext.StartSeasons.FirstOrDefaultAsync(u => u.Id == id);
            if (startSeason != null)
            {
                StartSeasonDTO startSeasonDTO = MapToDTO(startSeason);
                return startSeasonDTO;
            }
            return null;
        }

        public async Task<StartSeason?> GetByIdAsync(int id)
        {
            StartSeason? startSeason = await DbContext.StartSeasons.FirstOrDefaultAsync(u => u.Id == id);
            return startSeason;
        }

        public async Task<StartSeasonDTO?> UpdateAndReturnDTOAsync(StartSeason entity)
        {
            EntityEntry<StartSeason> result = DbContext.Set<StartSeason>().Update(entity);
            await DbContext.SaveChangesAsync();
            StartSeasonDTO startSeasonDTO = MapToDTO(result.Entity);
            return startSeasonDTO;
        }

        public async Task<StartSeason> UpdateAsync(StartSeason entity)
        {
            EntityEntry<StartSeason> result = DbContext.Set<StartSeason>().Update(entity);
            await DbContext.SaveChangesAsync();
            return result.Entity;
        }

        //Map to DTO method
        private StartSeasonDTO MapToDTO(StartSeason userEntity)
        {
            return new StartSeasonDTO
            {
                Id = userEntity.Id,
                Year = userEntity.Year,
                Season = userEntity.Season
            };
        }

        private List<StartSeasonDTO> MapToListOfDTOs(List<StartSeason> userEntities)
        {
            List<StartSeasonDTO> startSeasonDTOs = userEntities.Select(ua => new StartSeasonDTO
            {
                Id = ua.Id,
                Year = ua.Year,
                Season = ua.Season
            }).ToList();

            return startSeasonDTOs;
        }
    }
}
