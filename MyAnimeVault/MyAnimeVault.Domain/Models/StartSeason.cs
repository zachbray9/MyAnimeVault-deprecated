namespace MyAnimeVault.Domain.Models
{
    public class StartSeason
    {
        public int Id { get; set; }
        public int Year {  get; set; }
        public string Season { get; set; } = null!; 
        public ICollection<UserAnime> Animes { get; set; } = new List<UserAnime>();
    }
}
