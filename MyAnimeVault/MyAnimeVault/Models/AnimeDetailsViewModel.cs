using MyAnimeVault.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace MyAnimeVault.Models
{
    public class AnimeDetailsViewModel
    {
        [Required]
        public Anime Anime { get; set; } = null!;
        public User? CurrentUser { get; set; }
        public bool AnimeIsOnUsersList { get; set; } = false;
    }
}
