using MyAnimeVault.Domain.Models.Database;

namespace MyAnimeVault.Domain.Models
{
    public class Poster : DbObject
    {
        public string? Large { get; set; }
        public string Medium { get; set; } = null!;
        public ICollection<UserAnime> UserAnimes { get; set; } = new List<UserAnime>();
    }
}
