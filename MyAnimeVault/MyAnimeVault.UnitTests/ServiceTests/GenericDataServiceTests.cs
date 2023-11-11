using Microsoft.EntityFrameworkCore;
using Moq;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.Services.Database;
using System.Xml.Serialization;

namespace MyAnimeVault.UnitTests.ServiceTests
{
    [TestClass]
    public class GenericDataServiceTests
    {
        private MyAnimeVaultDbContext DbContext;
        private GenericDataService<UserAnime> UserAnimeService;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<MyAnimeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .EnableSensitiveDataLogging(true)
                .Options;

            DbContext = new MyAnimeVaultDbContext(options);
            UserAnimeService = new GenericDataService<UserAnime>(DbContext);

            await DbContext.Users.AddAsync(
                new User
                {
                    Id = 1,
                    Uid = "TestUid",
                    Email = "test@gmail.com",
                    DisplayName = "Test"
                }
            );

            await DbContext.Posters.AddAsync(
                new Poster
                {
                    Id = 1,
                    Large = "TestLarge1",
                    Medium = "TestMedium1"
                }
            );

            await DbContext.Posters.AddAsync(
                new Poster
                {
                    Id = 2,
                    Large = "TestLarge2",
                    Medium = "TestMedium2"
                }
            );

            await DbContext.Posters.AddAsync(
                new Poster
                {
                    Id = 3,
                    Large = "TestLarge3",
                    Medium = "TestMedium3"
                }
            );

            await DbContext.StartSeasons.AddAsync(
                new StartSeason
                {
                    Id = 1,
                    Season = "winter",
                    Year = 2021
                }
            );

            await DbContext.Animes.AddAsync(
                new UserAnime
                {
                    Id = 1,
                    Title = "TestName",
                    MediaType = "tv",
                    Rating = 9,
                    NumEpisodesWatched = 10,
                    TotalEpisodes = 47,
                    WatchStatus = "watching",
                    Status = "finished_airing",

                    UserId = 1,
                    PosterId = 1,
                    StartSeasonId = 1
                }
            );

            await DbContext.Animes.AddAsync(
                new UserAnime
                {
                    Id = 2,
                    Title = "TestName2",
                    MediaType = "movie",
                    Rating = 8,
                    NumEpisodesWatched = 1,
                    TotalEpisodes = 1,
                    WatchStatus = "completed",
                    Status = "finished_airing",

                    UserId = 1,
                    PosterId = 2,
                    StartSeasonId = 1
                }
            );

            await DbContext.Animes.AddAsync(
                new UserAnime
                {
                    Id = 3,
                    Title = "TestName3",
                    MediaType = "ova",
                    Rating = 7,
                    NumEpisodesWatched = 1,
                    TotalEpisodes = 1,
                    WatchStatus = "completed",
                    Status = "finished_airing",

                    UserId = 1,
                    PosterId = 3,
                    StartSeasonId = 1
                }
            );

            await DbContext.SaveChangesAsync();
        }



        [TestMethod]
        public async Task AddAsync_ShouldAddAndReturnUserAnime()
        {
            var userAnime = new UserAnime
            {
                Id = 115,
                Title = "TestName4",
                MediaType = "tv",
                Rating = 10,
                NumEpisodesWatched = 47,
                TotalEpisodes = 147,
                WatchStatus = "watching",
                Status = "currently_airing"
            };

            var result = await UserAnimeService.AddAsync(userAnime);

            Assert.IsNotNull(result);
            Assert.AreEqual(115, result.Id);
            Assert.AreEqual("TestName4", result.Title);
            Assert.AreEqual("tv", result.MediaType);
            Assert.AreEqual(10, result.Rating);
            Assert.AreEqual(47, result.NumEpisodesWatched);
            Assert.AreEqual(147, result.TotalEpisodes);
            Assert.AreEqual("watching", result.WatchStatus);
            Assert.AreEqual("currently_airing", result.Status);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnUserAnime()
        {
            var result = await UserAnimeService.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("TestName", result.Title);
            Assert.AreEqual("tv", result.MediaType);
            Assert.AreEqual(9, result.Rating);
            Assert.AreEqual(10, result.NumEpisodesWatched);
            Assert.AreEqual(47, result.TotalEpisodes);
            Assert.AreEqual("watching", result.WatchStatus);
            Assert.AreEqual("finished_airing", result.Status);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnListOfAllUserAnimes()
        {
            List<UserAnime>? userAnimes = await UserAnimeService.GetAllAsync();

            Assert.IsNotNull(userAnimes);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            bool result = await UserAnimeService.DeleteAsync(2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnUpdatedUserAnime()
        {
         
            UserAnime? existingUserAnime = await UserAnimeService.GetByIdAsync(1);
            existingUserAnime.Rating = 7;
            existingUserAnime.NumEpisodesWatched = 14;
            existingUserAnime.TotalEpisodes = 49;

            var result = await UserAnimeService.UpdateAsync(existingUserAnime);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(7, result.Rating);
            Assert.AreEqual(14, result.NumEpisodesWatched);
            Assert.AreEqual(49, result.TotalEpisodes);
        }



        [TestCleanup]
        public void Cleanup()
        {
            DbContext.Database.EnsureDeleted();
        }
    }
}
