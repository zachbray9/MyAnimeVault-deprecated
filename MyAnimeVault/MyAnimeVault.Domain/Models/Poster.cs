using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Models
{
    public class Poster
    {
        public Uri? Large { get; set; }
        public Uri Medium { get; set; } = null!;
    }
}
