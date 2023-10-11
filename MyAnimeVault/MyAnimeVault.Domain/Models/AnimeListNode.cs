using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Models
{
    public class AnimeListNode
    {
        [JsonProperty("node")]
        public Anime Anime { get; set; } = null!;
    }
}
