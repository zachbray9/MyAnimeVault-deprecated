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
            var entityType = DbContext.Model.FindEntityType(typeof(User));

            if (entityType == null)
            {
                return null;
            }

            var navigationProperties = entityType.GetNavigations();
            IQueryable<User> query = DbContext.Set<User>();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }

            return await query.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var entityType = DbContext.Model.FindEntityType(typeof(User));

            if (entityType == null)
            {
                return null;
            }

            var navigationProperties = entityType.GetNavigations();
            IQueryable<User> query = DbContext.Set<User>();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<User?> GetByUidAsync(string uid)
        {
            var entityType = DbContext.Model.FindEntityType(typeof(User));

            if (entityType == null)
            {
                return null;
            }

            var navigationProperties = entityType.GetNavigations();
            IQueryable<User> query = DbContext.Set<User>();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }

            return await query.FirstOrDefaultAsync(e => e.Uid == uid);
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
    }
}
