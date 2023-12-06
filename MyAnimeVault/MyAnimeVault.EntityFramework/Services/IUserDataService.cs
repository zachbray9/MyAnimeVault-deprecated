using MyAnimeVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.EntityFramework.Services
{
    public interface IUserDataService : IGenericDataService<User>
    {
        Task<User?> GetByUidAsync(string uid);
        Task<bool> AddAnimeToList(User user, UserAnime anime);
    }
}
