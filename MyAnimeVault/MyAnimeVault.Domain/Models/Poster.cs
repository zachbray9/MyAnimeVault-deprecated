namespace MyAnimeVault.Domain.Models
{
    public class Poster
    {
        public int Id { get; set; }
        public string? Large { get; set; }
        public string Medium { get; set; } = null!;
        public int AnimeId { get; set; }
        public UserAnime Anime { get; set; } = null!;
    }
}
