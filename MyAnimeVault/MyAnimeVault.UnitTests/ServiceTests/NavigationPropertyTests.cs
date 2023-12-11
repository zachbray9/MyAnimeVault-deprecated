using Microsoft.EntityFrameworkCore;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.Services.Database;
using MyAnimeVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyAnimeVault.EntityFramework.Services;

namespace MyAnimeVault.UnitTests.ServiceTests
{
    [TestClass]
    public class NavigationPropertyTests
    {
        private MyAnimeVaultDbContext DbContext;
        private UserDataService UserDataService;
        private GenericDataService<UserAnime> UserAnimeDataService;
        private GenericDataService<Poster> PosterDataService;
        private GenericDataService<StartSeason> StartSeasonDataService;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<MyAnimeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .EnableSensitiveDataLogging(true)
                .Options;

            DbContext = new MyAnimeVaultDbContext(options);
            UserDataService = new UserDataService(DbContext);
            PosterDataService = new GenericDataService<Poster>(DbContext);
            StartSeasonDataService = new GenericDataService<StartSeason>(DbContext);

            User testUser = new User
            {
                Uid = "testUid",
                Email = "test@gmail.com",
                DisplayName = "TestUser"
            };


            testUser = await UserDataService.AddAsync(testUser);

            Poster testExistingPoster = new Poster
            {
                Medium = "testMedium",
                Large = "testLarge"
            };

            testExistingPoster = await PosterDataService.AddAsync(testExistingPoster);

            StartSeason testExistingStartSeason = new StartSeason
            {
                Year = 2022,
                Season = "winter"
            };

            testExistingStartSeason = await StartSeasonDataService.AddAsync(testExistingStartSeason);

            UserAnime testExistingUserAnime = new UserAnime
            {
                AnimeId = 1,
                UserId = testUser.Id,
                Title = "Test Title",
                PosterId = testExistingPoster.Id,
                StartSeasonId = testExistingStartSeason.Id,
                MediaType = "tv",
                TotalEpisodes = 100,
                Status = "finished_airing"
            };

            await UserDataService.AddAnimeToList(testUser, testExistingUserAnime);
        }

        [TestMethod]
        public async Task AddAnimeToUserList_AnimeIsNotAlreadyInDatabase_PosterAndStartSeasonAreNotAlreadyInDatabase_ShouldReturnTrue()
        {
            User? user = await UserDataService.GetByUidAsync("testUid");

            Poster testPoster = new Poster
            {
                Medium = "testMedium2",
                Large = "testLarge2"
            };

            testPoster = await PosterDataService.AddAsync(testPoster);

            StartSeason testStartSeason = new StartSeason
            {
                Year = 2022,
                Season = "spring"
            };

            testStartSeason = await StartSeasonDataService.AddAsync(testStartSeason);

            UserAnime testUserAnime = new UserAnime
            {
                AnimeId = 2,
                UserId = user.Id,
                Title = "Test Title 2",
                PosterId = testPoster.Id,
                StartSeasonId = testStartSeason.Id,
                MediaType = "tv",
                TotalEpisodes = 67,
                Status = "currently_airing"
            };

            bool actualResult = await UserDataService.AddAnimeToList(user, testUserAnime);

            User? userWithNewUserAnime = await UserDataService.GetByUidAsync("testUid");
            UserAnime? newUserAnime = userWithNewUserAnime?.Animes.FirstOrDefault(ua => ua.AnimeId == 2);

            Assert.AreEqual(true, actualResult);
            Assert.IsNotNull(newUserAnime);
            Assert.AreEqual("testMedium2", newUserAnime?.Poster?.Medium);
            Assert.AreEqual("testLarge2", newUserAnime?.Poster?.Large);
            Assert.AreEqual(2022, newUserAnime?.StartSeason?.Year);
            Assert.AreEqual("spring", newUserAnime?.StartSeason?.Season);
        }

        [TestMethod]
        public async Task AddAnimeToUserList_AnimeIsNotAlreadyInDatabase_StartSeasonIsAlreadyInDatabase_ShouldReturnTrue()
        {
            User? user = await UserDataService.GetByUidAsync("testUid");

            Poster testPoster = new Poster
            {
                Medium = "testMedium2",
                Large = "testLarge2"
            };

            testPoster = await PosterDataService.AddAsync(testPoster);

            StartSeason? existingStartSeason = await DbContext.StartSeasons.FirstOrDefaultAsync(ss => ss.Year == 2022 && ss.Season == "winter");

            UserAnime testUserAnime = new UserAnime
            {
                AnimeId = 2,
                UserId = user.Id,
                Title = "Test Title 2",
                PosterId = testPoster.Id,
                StartSeasonId = existingStartSeason.Id,
                MediaType = "tv",
                TotalEpisodes = 67,
                Status = "currently_airing"
            };

            bool actualResult = await UserDataService.AddAnimeToList(user, testUserAnime);

            Assert.AreEqual(true, actualResult);
        }

        [TestMethod]
        public async Task GetAnimeFromUserList_AllPropertiesShouldBeEqualToUserAnimeInSetup()
        {
            User? user = await UserDataService.GetByUidAsync("testUid");
            UserAnime? expectedUserAnime = await DbContext.Animes.FirstOrDefaultAsync(ua => ua.AnimeId == 1);
            Poster? expectedPoster = await DbContext.Posters.FirstOrDefaultAsync(p => p.Medium == "testMedium");
            StartSeason? expectedStartSeason = await DbContext.StartSeasons.FirstOrDefaultAsync(ss => ss.Year == 2022 && ss.Season == "winter");

            UserAnime? userAnimeNavigationProperty = user.Animes.FirstOrDefault(ua => ua.AnimeId == 1);

            Assert.AreEqual(expectedUserAnime.Id, userAnimeNavigationProperty.Id);
            Assert.AreEqual(user.Id, userAnimeNavigationProperty.UserId);
            Assert.AreEqual(1, userAnimeNavigationProperty.AnimeId);
            Assert.AreEqual("Test Title", userAnimeNavigationProperty.Title);
            Assert.AreEqual("testMedium", userAnimeNavigationProperty.Poster?.Medium);
            Assert.AreEqual(expectedPoster.Id, userAnimeNavigationProperty.PosterId);
            Assert.AreEqual(2022, userAnimeNavigationProperty.StartSeason?.Year);
            Assert.AreEqual("winter", userAnimeNavigationProperty.StartSeason?.Season);
            Assert.AreEqual(expectedStartSeason.Id, userAnimeNavigationProperty.StartSeasonId);
            Assert.AreEqual("tv", userAnimeNavigationProperty.MediaType);
            Assert.AreEqual(100, userAnimeNavigationProperty.TotalEpisodes);
            Assert.AreEqual("finished_airing", userAnimeNavigationProperty.Status);
            Assert.AreEqual(0, userAnimeNavigationProperty.Rating);
            Assert.AreEqual(0, userAnimeNavigationProperty.NumEpisodesWatched);
            Assert.AreEqual("watching", userAnimeNavigationProperty.WatchStatus);
        }
    }
}
