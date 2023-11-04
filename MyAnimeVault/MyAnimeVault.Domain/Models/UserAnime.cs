namespace MyAnimeVault.Domain.Models
{
    public class UserAnime
    {
        public int Id { get; set; } //Id from myanimelist api
        public ICollection<User> Users { get; set; } = new List<User>();
        public string Title { get; set; } = null!;

        public int PosterId { get; set; }
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
