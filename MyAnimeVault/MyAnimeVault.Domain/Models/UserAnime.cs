using MyAnimeVault.Domain.Models.Database;

namespace MyAnimeVault.Domain.Models
{
    public class UserAnime : DbObject
    {
        public int AnimeId { get; set; } //Anime Id from MyAnimeList Api
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int PosterId { get; set; }
        public Poster? Poster { get; set; }
        public int StartSeasonId { get; set; }
        public StartSeason? StartSeason { get; set; }
        public string MediaType { get; set; } = null!;
        public int Rating { get; set; } = 0;
        public int NumEpisodesWatched { get; set; } = 0;
        public int TotalEpisodes {  get; set; }
        public string WatchStatus { get; set; } = "watching";
        public string Status { get; set; } = null!;
    }
}
