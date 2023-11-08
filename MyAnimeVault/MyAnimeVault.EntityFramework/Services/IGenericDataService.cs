namespace MyAnimeVault.EntityFramework.Services
{
    public interface IGenericDataService<T>
    {
        Task<T> AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>?> GetAllAsync();
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
