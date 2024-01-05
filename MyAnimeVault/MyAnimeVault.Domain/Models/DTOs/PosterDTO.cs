namespace MyAnimeVault.Domain.Models.DTOs
{
    public class PosterDTO
    {
        public int Id { get; set; }
        public string? Large { get; set; }
        public string Medium { get; set; } = null!;
    }
}
