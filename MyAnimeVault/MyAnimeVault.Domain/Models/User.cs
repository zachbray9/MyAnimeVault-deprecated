using MyAnimeVault.Domain.Models.Database;

namespace MyAnimeVault.Domain.Models
{
    public class User : DbObject
    {
        // Id will be the same is Firebase Uid
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public ICollection<UserAnime> Animes { get; set; } = new List<UserAnime>(); 
    }
}
