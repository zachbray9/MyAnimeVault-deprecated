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
        [TestMethod]
        public async Task GetByIdTest()
        {
            var mockDbContext = new Mock<MyAnimeVaultDbContext>();
            var mockDbSet = new Mock<DbSet<User>>();

            mockDbContext.Setup(context => context.Set<User>()).Returns(mockDbSet.Object);

            var UserService = new GenericDataService<User>(mockDbContext.Object);
            var testUser = new User
            {
                Id = 1,
                DisplayName = "Test",
                Email = "test@gmail.com"
            };

            mockDbSet.Setup(dbSet => dbSet.Find(It.IsAny<object[]>())).Returns(testUser);

            var result = await UserService.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("test@gmail.com", result.Email);
        }
    }
}
