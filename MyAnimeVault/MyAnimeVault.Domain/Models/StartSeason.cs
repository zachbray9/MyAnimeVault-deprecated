using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Models
{
    public class StartSeason
    {
        public int Year {  get; set; }
        public string Season { get; set; } = null!; 
    }
}
