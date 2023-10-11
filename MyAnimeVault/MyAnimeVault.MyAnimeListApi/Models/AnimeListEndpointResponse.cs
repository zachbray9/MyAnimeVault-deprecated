using MyAnimeVault.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.MyAnimeListApi.Models
{
    public class AnimeListEndpointResponse
    {
        public List<AnimeListNode> Data { get; set; } = new List<AnimeListNode>();
    }
}
