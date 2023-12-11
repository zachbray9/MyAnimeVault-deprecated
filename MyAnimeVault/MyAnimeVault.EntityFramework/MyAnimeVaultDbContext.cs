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

        //need a constructor with no parameters for unit testing
        public MyAnimeVaultDbContext(){} 

        public MyAnimeVaultDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Users

            modelBuilder.Entity<User>()
                .HasMany(u => u.Animes)
                .WithOne(ua => ua.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            //User Animes

            modelBuilder.Entity<UserAnime>()
                .HasOne(ua => ua.StartSeason)
                .WithMany(ss => ss.Animes)
                .HasForeignKey(ua => ua.StartSeasonId)
                .IsRequired(false);

            modelBuilder.Entity<UserAnime>()
                .HasOne(ua => ua.Poster)
                .WithMany(p => p.UserAnimes)
                .HasForeignKey(ua => ua.PosterId)
                .IsRequired(false);
        }
    }
}
