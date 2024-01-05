namespace MyAnimeVault.Domain.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Uid { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<UserAnimeDTO> Animes { get; set; } = new List<UserAnimeDTO>();
    }
}
