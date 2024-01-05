using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.EntityFramework;
using System.Xml.Linq;

namespace MyAnimeVault.RestApi.Services
{
    public class UserAnimeDataService : IUserAnimeDataService
    {
        private readonly MyAnimeVaultDbContext DbContext;

        public UserAnimeDataService(MyAnimeVaultDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<UserAnimeDTO?> AddAndReturnDTOAsync(UserAnime entity)
        {
            EntityEntry<UserAnime> createdResult = await DbContext.Set<UserAnime>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            UserAnimeDTO userAnimeDTO = MapToDTO(createdResult.Entity);
            return userAnimeDTO;
        }

        public async Task<UserAnime> AddAsync(UserAnime entity)
        {
            EntityEntry<UserAnime> createdResult = await DbContext.Set<UserAnime>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            UserAnime? entity = await DbContext.Set<UserAnime>().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return false;
            }

            DbContext.Set<UserAnime>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserAnimeDTO>?> GetAllAsDTOsAsync()
        {
            List<UserAnimeDTO> userAnimeDTOs = MapToListOfDTOs(await DbContext.Animes.Include(ua => ua.Poster).Include(ua => ua.StartSeason).ToListAsync());
            return userAnimeDTOs;
        }

        public async Task<List<UserAnime>?> GetAllAsync()
        {
            List<UserAnime>? userAnimes = await DbContext.Animes.Include(ua => ua.Poster).Include(ua => ua.StartSeason).ToListAsync();
            return userAnimes;
        }

        public async Task<UserAnimeDTO?> GetByIdAsDTOAsync(int id)
        {
            UserAnime? userAnime = await DbContext.Animes.Include(ua => ua.Poster).Include(ua => ua.StartSeason).FirstOrDefaultAsync(u => u.Id == id);
            if (userAnime != null)
            {
                UserAnimeDTO userAnimeDTO = MapToDTO(userAnime);
                return userAnimeDTO;
            }
            return null;
        }

        public async Task<UserAnime?> GetByIdAsync(int id)
        {
            UserAnime? user = await DbContext.Animes.Include(ua => ua.Poster).Include(ua => ua.StartSeason).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<UserAnimeDTO?> UpdateAndReturnDTOAsync(UserAnime entity)
        {
            EntityEntry<UserAnime> result = DbContext.Set<UserAnime>().Update(entity);
            await DbContext.SaveChangesAsync();
            UserAnimeDTO userAnimeDTO = MapToDTO(result.Entity);
            return userAnimeDTO;
        }

        public async Task<UserAnime> UpdateAsync(UserAnime entity)
        {
            EntityEntry<UserAnime> result = DbContext.Set<UserAnime>().Update(entity);
            await DbContext.SaveChangesAsync();
            return result.Entity;
        }

        //Map to DTO method
        private UserAnimeDTO MapToDTO(UserAnime userEntity)
        {
            return new UserAnimeDTO
            {
                Id = userEntity.Id,
                AnimeId = userEntity.AnimeId,
                Title = userEntity.Title,
                MediaType = userEntity.MediaType,
                Rating = userEntity.Rating,
                NumEpisodesWatched = userEntity.NumEpisodesWatched,
                TotalEpisodes = userEntity.TotalEpisodes,
                WatchStatus = userEntity.WatchStatus,
                Status = userEntity.Status,
                Poster = userEntity.Poster != null ? new PosterDTO
                {
                    Id = userEntity.Poster.Id,
                    Large = userEntity.Poster.Large,
                    Medium = userEntity.Poster.Medium
                } : null,
                StartSeason = userEntity.StartSeason != null ? new StartSeasonDTO
                {
                    Id = userEntity.StartSeason.Id,
                    Year = userEntity.StartSeason.Year,
                    Season = userEntity.StartSeason.Season
                } : null
            };
        }

        private List<UserAnimeDTO> MapToListOfDTOs(List<UserAnime> userAnimes)
        {
            List<UserAnimeDTO> userAnimeDTOs = userAnimes.Select(ua => new UserAnimeDTO
            {
                Id = ua.Id,
                AnimeId = ua.AnimeId,
                Title = ua.Title,
                MediaType = ua.MediaType,
                Rating = ua.Rating,
                NumEpisodesWatched = ua.NumEpisodesWatched,
                TotalEpisodes = ua.TotalEpisodes,
                WatchStatus = ua.WatchStatus,
                Status = ua.Status,
                Poster = ua.Poster != null ? new PosterDTO
                {
                    Id = ua.Poster.Id,
                    Large = ua.Poster.Large,
                    Medium = ua.Poster.Medium
                } : null,
                StartSeason = ua.StartSeason != null ? new StartSeasonDTO
                {
                    Id = ua.StartSeason.Id,
                    Year = ua.StartSeason.Year,
                    Season = ua.StartSeason.Season
                } : null
            }).ToList();

            return userAnimeDTOs;
        }
    }
}
