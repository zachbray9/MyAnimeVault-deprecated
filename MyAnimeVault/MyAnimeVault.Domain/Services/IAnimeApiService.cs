using MyAnimeVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Services
{
    public interface IAnimeApiService
    {
        Task<List<Anime>> GetAllAnime();
    }
}
