using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.EntityFramework.Services;

namespace MyAnimeVault.Services.Database
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

        public async Task<User?> GetByIdAsync(int id)
        {
            User? user = await DbContext.Users.Include(u => u.Animes).ThenInclude(a => a.Poster).Include(u => u.Animes).ThenInclude(a => a.StartSeason).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User?> GetByUidAsync(string uid)
        {
            User? user = await DbContext.Users.Include(u => u.Animes).ThenInclude(a => a.Poster).Include(u => u.Animes).ThenInclude(a => a.StartSeason).FirstOrDefaultAsync(u => u.Uid == uid);
            return user;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            EntityEntry<User> result = DbContext.Set<User>().Update(entity);
            await DbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> AddAnimeToList(User user, UserAnime anime)
        {
            if(user == null || user.Animes.Any(ua => ua.AnimeId == anime.Id))
            {
                return false;
            }

            user.Animes.Add(anime);
            await DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAnimeFromList(User user, UserAnime anime)
        {
            if(user == null || !(user.Animes.Any(ua => ua.Id == anime.Id)))
            {
                return false;
            }

            user.Animes.Remove(anime);
            await DbContext.SaveChangesAsync();

            return true;
        }
    }
}
