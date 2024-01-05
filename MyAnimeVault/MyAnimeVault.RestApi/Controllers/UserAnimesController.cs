using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.RestApi.Services;
using System.Runtime.CompilerServices;

namespace MyAnimeVault.RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAnimesController : Controller
    {
        private readonly IUserAnimeDataService UserAnimeDataService;

        public UserAnimesController(IUserAnimeDataService userAnimeDataService)
        {
            UserAnimeDataService = userAnimeDataService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserAnimeById([FromRoute] int id)
        {
            UserAnimeDTO? userAnime = await UserAnimeDataService.GetByIdAsDTOAsync(id);
            if (userAnime != null)
            {
                return Ok(userAnime);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAnimes()
        {
            List<UserAnimeDTO>? UserAnimes = await UserAnimeDataService.GetAllAsDTOsAsync();
            return Ok(UserAnimes);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAnime(UserAnimeDTO userAnime)
        {
            UserAnimeDTO? userAnimeDTO = await UserAnimeDataService.AddAndReturnDTOAsync(userAnime);
            return Ok(userAnimeDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAnime(UserAnimeDTO userAnime)
        {
            UserAnimeDTO? userAnimeDTO = await UserAnimeDataService.UpdateAndReturnDTOAsync(userAnime);
            return Ok(userAnimeDTO);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteUserAnime([FromRoute] int id)
        {
            bool success = await UserAnimeDataService.DeleteAsync(id);

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
