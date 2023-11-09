using Microsoft.EntityFrameworkCore;
using Moq;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.Services.Database;

namespace MyAnimeVault.UnitTests.ServiceTests
{
    [TestClass]
    public class GenericDataServiceTests
    {
        private MyAnimeVaultDbContext DbContext;
        private GenericDataService<User> UserService;

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<MyAnimeVaultDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;

            DbContext = new MyAnimeVaultDbContext(options);
            UserService = new GenericDataService<User>(DbContext);

            await DbContext.Users.AddAsync(
                new User
                {
                    Id = 1,
                    Email = "default@gmail.com",
                    DisplayName = "Default"
                }
            );

            await DbContext.Users.AddAsync(
                new User
                {
                    Id = 2,
                    Email = "default2@gmail.com",
                    DisplayName = "Default2"
                }
            );

            await DbContext.Users.AddAsync(
                new User
                {
                    Id = 3,
                    Email = "default3@gmail.com",
                    DisplayName = "Default3"
                }
            );

            await DbContext.SaveChangesAsync();
        }



        [TestMethod]
        public async Task AddAsync_ShouldAddAndReturnUser()
        {
            var user = new User
            {
                Id = 115,
                Email = "test@gmail.com",
                DisplayName = "Test"
            };

            var result = await UserService.AddAsync(user);

            Assert.IsNotNull(result);
            Assert.AreEqual(115, result.Id);
            Assert.AreEqual("test@gmail.com", result.Email);
            Assert.AreEqual("Test", result.DisplayName);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            var user = await UserService.GetByIdAsync(1);

            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Id);
            Assert.AreEqual("default@gmail.com", user.Email);
            Assert.AreEqual("Default", user.DisplayName);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnListOfAllUsers()
        {
            List<User>? users = await UserService.GetAllAsync();

            Assert.IsNotNull(users);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            bool result = await UserService.DeleteAsync(1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnUpdatedUser()
        {
         
            User? existingUser = await UserService.GetByIdAsync(2);
            existingUser.Email = "updatedEmail@gmail.com"; 
            existingUser.DisplayName = "UpdatedDisplayName";

            var result = await UserService.UpdateAsync(existingUser);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("updatedEmail@gmail.com", result.Email);
            Assert.AreEqual("UpdatedDisplayName", result.DisplayName);
        }



        [TestCleanup]
        public void Cleanup()
        {
            DbContext.Database.EnsureDeleted();
        }
    }
}
