using Newtonsoft.Json;

namespace MyAnimeVault.Domain.Models
{
    public class Anime
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        [JsonProperty("main_picture")]
        public Poster? Picture { get; set; }
        [JsonProperty("alternative_titles")]
        public AlternativeTitles? AlternativeTitles { get; set; }
        [JsonProperty("start_date")]
        public string? StartDate { get; set; }
        [JsonProperty("end_date")]
        public string? EndDate { get; set; }
        public string? Synopsis { get; set; }
        [JsonProperty("mean")]
        public float? MeanScore { get; set; }
        public int? Rank { get; set; }
        public int? Popularity { get; set; }
        public string? Nsfw { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime LastUpdatedAt { get; set; }
        [JsonProperty("media_type")]
        public string MediaType { get; set; } = null!;
        public string Status { get; set; } = null!;
        [JsonProperty("num_episodes")]
        public int NumEpisodes { get; set; }
        [JsonProperty("start_season")]
        public StartSeason? StartSeason { get; set; }
        public string? Source { get; set; }
        [JsonProperty("broadcast")]
        public BroadcastDate? BroadcastDate{ get; set; }
        [JsonProperty("average_episode_duration")]
        public int? AverageEpisodeDuration { get; set; }
        public string? Rating { get; set; }
        public ICollection<Studio> Studios { get; set; } = new List<Studio>();
        [JsonProperty("opening_themes")]
        public ICollection<OpeningSong> OpeningSongs { get; set; } = new List<OpeningSong>();
        [JsonProperty("ending_themes")]
        public ICollection<EndingSong> EndingSongs { get; set;} = new List<EndingSong>();
    }
}
