using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
