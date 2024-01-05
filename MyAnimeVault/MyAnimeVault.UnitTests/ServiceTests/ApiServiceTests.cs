using MyAnimeVault.Domain.Services.Api.Database;
using MyAnimeVault.Services.Api.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeVault.UnitTests.ServiceTests
{
    [TestClass]
    public class ApiServiceTests
    {
        private readonly UserApiService UserApiService;

        [TestInitialize]
        public async Task Setup()
        {
            
        }

        [TestMethod]
        public async Task GetUserById_ShouldReturnUserDTO()
        {

        }
    }
}
