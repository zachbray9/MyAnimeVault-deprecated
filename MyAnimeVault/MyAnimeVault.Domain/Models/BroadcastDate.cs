using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.Domain.Models
{
    public class BroadcastDate
    {
        public int Id { get; set; }
        public string Day { get; set; } = null!;
        public string? Time { get; set; }
    }
}
