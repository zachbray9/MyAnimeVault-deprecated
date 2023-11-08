using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyAnimeVault.Domain.Models.Database;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.EntityFramework.Services;

namespace MyAnimeVault.Services.Database
{
    public class GenericDataService<T> : IGenericDataService<T> where T : DbObject
    {
        private readonly MyAnimeVaultDbContext DbContext;

        public GenericDataService(MyAnimeVaultDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            EntityEntry<T> createdResult = await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            T? entity = await DbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return false;
            }

            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>?> GetAllAsync()
        {
            var entityType = DbContext.Model.FindEntityType(typeof(T));

            if(entityType == null)
            {
                return null;
            }

            var navigationProperties = entityType.GetNavigations();
            IQueryable<T> query = DbContext.Set<T>();

            foreach(var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entityType = DbContext.Model.FindEntityType(typeof(T));

            if(entityType == null)
            {
                return null;
            }

            var navigationProperties = entityType.GetNavigations();
            IQueryable<T> query = DbContext.Set<T>();

            foreach(var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty.Name);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            EntityEntry<T> result = DbContext.Set<T>().Update(entity);
            await DbContext.SaveChangesAsync();
            return result.Entity;
        }
    }
}
