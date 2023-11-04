using Newtonsoft.Json;

namespace MyAnimeVault.Domain.Models
{
    public class EndingSong
    {
        public int Id { get; set; }
        [JsonProperty("anime_id")]
        public int AnimeId { get; set; }
        public string Text { get; set; } = null!;
    }
}
