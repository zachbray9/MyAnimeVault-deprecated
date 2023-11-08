using Microsoft.EntityFrameworkCore;
using MyAnimeVault.Domain.Models;

namespace MyAnimeVault.EntityFramework
{
    public class MyAnimeVaultDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserAnime> Animes { get; set; }
        public DbSet<Poster> Posters { get; set; }
        public DbSet<StartSeason> StartSeasons { get; set; }

        public MyAnimeVaultDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Users

            modelBuilder.Entity<User>()
                .HasMany(u => u.Animes)
                .WithMany(ua => ua.Users);

            //User Animes

            modelBuilder.Entity<UserAnime>()
                .HasOne(ua => ua.StartSeason)
                .WithMany(ss => ss.Animes)
                .HasForeignKey(ua => ua.StartSeasonId)
                .IsRequired();

            modelBuilder.Entity<UserAnime>()
                .HasOne(ua => ua.Poster)
                .WithOne(p => p.Anime)
                .HasForeignKey<Poster>(p => p.AnimeId)
                .IsRequired();
        }
    }
}
