using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.EntityFramework.Services;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace MyAnimeVault.RestApi.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly MyAnimeVaultDbContext DbContext;
        private readonly IUserAnimeDataService UserAnimeDataService;

        public UserDataService(MyAnimeVaultDbContext dbContext, IUserAnimeDataService userAnimeDataService)
        {
            DbContext = dbContext;
            UserAnimeDataService = userAnimeDataService;
        }

        public async Task<User> AddAsync(User entity)
        {
            EntityEntry<User> createdResult = await DbContext.Set<User>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<UserDTO?> AddAndReturnDTOAsync(UserDTO newUser)
        {
            User? user = await DbContext.Users.FirstOrDefaultAsync(u => u.Uid == newUser.Uid);
            if (user == null)
            {
                user = new User
                {
                    Uid = newUser.Uid,
                    Email = newUser.Email,
                    DisplayName = newUser.DisplayName
                };

                EntityEntry<User> createdResult = await DbContext.Set<User>().AddAsync(user);
                await DbContext.SaveChangesAsync();
                UserDTO userDTO = MapToDTO(createdResult.Entity);
                return userDTO;
            }

            return null;
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

        public async Task<UserDTO?> UpdateAndReturnDTOAsync(UserDTO userDTO)
        {
            User? user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userDTO.Id);

            if(user != null)
            {
                user.Uid = userDTO.Uid;
                user.Email = userDTO.Email;
                user.DisplayName = userDTO.DisplayName;

                EntityEntry<User> result = DbContext.Set<User>().Update(user);
                await DbContext.SaveChangesAsync();
                userDTO = MapToDTO(result.Entity);
                return userDTO;
            }

            return null;
        }

        public async Task<bool> AddAnimeToListAsync(int userId, UserAnimeDTO userAnimeDTO)
        {
            User? user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Animes.Any(ua => ua.AnimeId == userAnimeDTO.Id))
            {
                return false;
            }

            Poster? existingPoster = null;
            StartSeason? existingStartSeason = null;

            if(userAnimeDTO.Poster != null)
            {
                existingPoster = await DbContext.Posters.FirstOrDefaultAsync(p => p.Medium == userAnimeDTO.Poster.Medium);
                if(existingPoster == null)
                {
                    EntityEntry<Poster> createdEntity = await DbContext.Posters.AddAsync(new Poster
                    {
                        Large = userAnimeDTO.Poster.Large,
                        Medium = userAnimeDTO.Poster.Medium
                    });

                    await DbContext.SaveChangesAsync();
                    existingPoster = createdEntity.Entity;

                }
            }

            if (userAnimeDTO.StartSeason != null)
            {
                existingStartSeason = await DbContext.StartSeasons.FirstOrDefaultAsync(ss => ss.Year == userAnimeDTO.StartSeason.Year && ss.Season == userAnimeDTO.StartSeason.Season);
                if (existingStartSeason == null)
                {
                    EntityEntry<StartSeason> createdEntity = await DbContext.StartSeasons.AddAsync(new StartSeason
                    {
                        Year = userAnimeDTO.StartSeason.Year,
                        Season = userAnimeDTO.StartSeason.Season,
                    });

                    await DbContext.SaveChangesAsync();
                    existingStartSeason = createdEntity.Entity;
                }
            }

            user.Animes.Add(new UserAnime
            {
                AnimeId = userAnimeDTO.AnimeId,
                UserId = user.Id,
                Title = userAnimeDTO.Title,
                PosterId = existingPoster != null ? existingPoster.Id : null,
                StartSeasonId = existingStartSeason != null ? existingStartSeason.Id : null,
                MediaType = userAnimeDTO.MediaType,
                TotalEpisodes = userAnimeDTO.TotalEpisodes,
                Status = userAnimeDTO.Status
            });

            await DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAnimeFromListAsync(int userId, int userAnimeId)
        {
            User? user = await GetByIdAsync(userId);
            UserAnime? animeToRemove = await UserAnimeDataService.GetByIdAsync(userAnimeId);

            if (user != null && animeToRemove != null && user.Animes.Any(ua => ua.AnimeId == animeToRemove.AnimeId))
            {
                user.Animes.Remove(animeToRemove);
                await DbContext.SaveChangesAsync();
                return true;
            }

            return false;
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
                    Poster = anime.Poster != null ? new PosterDTO
                    {
                        Id = anime.Poster.Id,
                        Large = anime.Poster.Large,
                        Medium = anime.Poster.Medium
                    } : null,
                    StartSeason = anime.StartSeason != null ? new StartSeasonDTO
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
                }).ToList()
            }).ToList();

            return userDTOs;
        }
    }
}
