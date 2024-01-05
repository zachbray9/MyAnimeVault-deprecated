using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MyAnimeVault.Models
{
    public class AnimeDetailsViewModel
    {
        [Required]
        public Anime Anime { get; set; } = null!;
        public UserDTO? CurrentUser { get; set; }
        public UserAnimeDTO? UserAnime { get; set; }
    }
}
