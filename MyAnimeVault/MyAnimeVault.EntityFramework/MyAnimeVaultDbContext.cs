using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyAnimeVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
        }
    }
}
