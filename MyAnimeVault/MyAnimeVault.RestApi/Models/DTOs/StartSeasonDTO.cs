namespace MyAnimeVault.RestApi.Models.DTOs
{
    public class StartSeasonDTO
    {
        public int Id {  get; set; }
        public int Year { get; set; }
        public string Season { get; set; } = null!;
    }
}
