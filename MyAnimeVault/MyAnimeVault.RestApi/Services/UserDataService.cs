using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.EntityFramework.Services;
using MyAnimeVault.RestApi.Models.DTOs;
using System.Xml.Linq;

namespace MyAnimeVault.RestApi.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly MyAnimeVaultDbContext DbContext;

        public UserDataService(MyAnimeVaultDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<User> AddAsync(User entity)
        {
            EntityEntry<User> createdResult = await DbContext.Set<User>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<UserDTO> AddAndReturnDTOAsync(User entity)
        {
            EntityEntry<User> createdResult = await DbContext.Set<User>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            UserDTO userDTO = MapToDTO(createdResult.Entity);
            return userDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            User? entity = await DbContext.Set<User>().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return false;
            }

            DbContext.Set<User>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>?> GetAllAsync()
        {
            List<User>? users = await DbContext.Users.Include(u => u.Animes).ThenInclude(a => a.Poster).Include(u => u.Animes).ThenInclude(a => a.StartSeason).ToListAsync();
            return users;
        }

        public async Task<List<UserDTO>?> GetAllAsDTOsAsync()
        {
            List<UserDTO>? userDTOs = MapToListOfDTOs(await DbContext.Users.Include(u => u.Animes).ThenInclude(a => a.Poster).Include(u => u.Animes).ThenInclude(a => a.StartSeason).ToListAsync());
            return userDTOs;

        }

        public async Task<User?> GetByIdAsync(int id)
        {
            User? user = await DbContext.Users.Include(u => u.Animes).ThenInclude(a => a.Poster).Include(u => u.Animes).ThenInclude(a => a.StartSeason).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<UserDTO?> GetByIdAsDTOAsync(int id)
        {
            User? user = await DbContext.Users.Include(u => u.Animes).ThenInclude(a => a.Poster).Include(u => u.Animes).ThenInclude(a => a.StartSeason).FirstOrDefaultAsync(u => u.Id == id);

            if(user != null)
            {
                UserDTO userDTO = MapToDTO(user);
                return userDTO;
            }

            return null;
        }

        public async Task<User?> GetByUidAsync(string uid)
        {
            User? user = await DbContext.Users.Include(u => u.Animes).ThenInclude(a => a.Poster).Include(u => u.Animes).ThenInclude(a => a.StartSeason).FirstOrDefaultAsync(u => u.Uid == uid);
            return user;
        }

        public async Task<UserDTO?> GetByUidAsDTOAsync(string uid)
        {
            User? user = await DbContext.Users.Include(u => u.Animes).ThenInclude(a => a.Poster).Include(u => u.Animes).ThenInclude(a => a.StartSeason).FirstOrDefaultAsync(u => u.Uid == uid);

            if(user != null)
            {
                UserDTO userDTO= MapToDTO(user);
                return userDTO;
            }

            return null;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            EntityEntry<User> result = DbContext.Set<User>().Update(entity);
            await DbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<UserDTO> UpdateAndReturnDTOAsync(User entity)
        {
            EntityEntry<User> result = DbContext.Set<User>().Update(entity);
            await DbContext.SaveChangesAsync();
            UserDTO userDTO = MapToDTO(result.Entity);
            return userDTO;
        }

        public async Task<bool> AddAnimeToList(User user, UserAnime anime)
        {
            if (user == null || user.Animes.Any(ua => ua.AnimeId == anime.Id))
            {
                return false;
            }

            user.Animes.Add(anime);
            await DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAnimeFromList(User user, UserAnime anime)
        {
            if (user == null || !(user.Animes.Any(ua => ua.Id == anime.Id)))
            {
                return false;
            }

            user.Animes.Remove(anime);
            await DbContext.SaveChangesAsync();

            return true;
        }

        //Map to DTO method
        private UserDTO MapToDTO(User userEntity) 
        {
            return new UserDTO
            {
                Id = userEntity.Id,
                Uid = userEntity.Uid,
                DisplayName = userEntity.DisplayName,
                Email = userEntity.Email,
                Animes = userEntity.Animes.Select(anime => new UserAnimeDTO
                {
                    Id = anime.Id,
                    AnimeId = anime.AnimeId,
                    Title = anime.Title,
                    MediaType = anime.MediaType,
                    Rating = anime.Rating,
                    NumEpisodesWatched = anime.NumEpisodesWatched,
                    TotalEpisodes = anime.TotalEpisodes,
                    WatchStatus = anime.WatchStatus,
                    Status = anime.Status,
                    PosterDTO = anime.Poster != null ? new PosterDTO
                    {
                        Id = anime.Poster.Id,
                        Large = anime.Poster.Large,
                        Medium = anime.Poster.Medium
                    } : null,
                    StartSeasonDTO = anime.StartSeason != null ? new StartSeasonDTO
                    {
                        Id = anime.StartSeason.Id,
                        Year = anime.StartSeason.Year,
                        Season = anime.StartSeason.Season
                    } : null,
                }).ToList()
            };
        }

        private List<UserDTO> MapToListOfDTOs(List<User> users)
        {
            List<UserDTO> userDTOs = users.Select(user => new UserDTO
            {
                Id = user.Id,
                Uid = user.Uid,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Animes = user.Animes.Select(ua => new UserAnimeDTO 
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
                    
                    PosterDTO = ua.Poster != null ? new PosterDTO
                    {
                        Id = ua.Poster.Id,
                        Large = ua.Poster.Large,
                        Medium = ua.Poster.Medium
                    } : null,
                    StartSeasonDTO = ua.StartSeason != null ? new StartSeasonDTO
                    {
                        Id = ua.StartSeason.Id,
                        Year = ua.StartSeason.Year,
                        Season = ua.StartSeason.Season
                    } : null
                }).ToList()
            }).ToList();

            return userDTOs;
        }
    }
}
