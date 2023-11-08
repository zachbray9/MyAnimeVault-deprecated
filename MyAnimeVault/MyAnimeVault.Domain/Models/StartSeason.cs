using MyAnimeVault.Domain.Models.Database;

namespace MyAnimeVault.Domain.Models
{
    public class StartSeason : DbObject
    {
        public int Year {  get; set; }
        public string Season { get; set; } = null!; 
        public ICollection<UserAnime> Animes { get; set; } = new List<UserAnime>();
    }
}
