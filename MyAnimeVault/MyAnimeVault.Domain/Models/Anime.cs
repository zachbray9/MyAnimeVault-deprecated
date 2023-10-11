using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Models
{
    public class Anime
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        [JsonProperty("main_picture")]
        public Poster? Picture { get; set; }
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
        public Genre[] Genres { get; set; } = null!;
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime LastUpdatedAt { get; set; }
        [JsonProperty("media_type")]
        public string MediaType { get; set; } = null!;
        public string Status { get; set; } = null!;
        [JsonProperty("num_episodes")]
        public int NumEpisodes { get; set; }
        public string? Source { get; set; }
        [JsonProperty("broadcast")]
        public BroadcastDate? BroadcastDate{ get; set; }
        [JsonProperty("average_episode_duration")]
        public int? AverageEpisodeDuration { get; set; }
        public string? Rating { get; set; }
        public Studio[] Studios { get; set; } = null!;
    }
}
