namespace MyAnimeVault.Domain.Models
{
    public class User
    {
        public int Id { get; set; } // Firebase Uid
        public ICollection<UserAnime> Animes { get; set; } = new List<UserAnime>(); 
    }
}
