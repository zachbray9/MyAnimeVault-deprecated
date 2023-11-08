using MyAnimeVault.Domain.Models.Database;

namespace MyAnimeVault.Domain.Models
{
    public class UserAnime : DbObject
    {
        //Id property will be the same from myanimelist api
        public ICollection<User> Users { get; set; } = new List<User>();
        public string Title { get; set; } = null!;
        public Poster? Poster { get; set; }
        public int StartSeasonId { get; set; }
        public StartSeason? StartSeason { get; set; }
        public string MediaType { get; set; } = null!;
        public int Rating { get; set; }
        public int NumEpisodesWatched { get; set; } = 0;
        public int TotalEpisodes {  get; set; }
        public string? WatchStatus { get; set; }
        public string Status { get; set; } = null!;
    }
}
