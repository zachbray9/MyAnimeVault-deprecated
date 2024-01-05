using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.RestApi.Services;

namespace MyAnimeVault.RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StartSeasonsController : Controller
    {
        private readonly IStartSeasonDataService StartSeasonDataService;

        public StartSeasonsController(IStartSeasonDataService startSeasonDataService)
        {
            StartSeasonDataService = startSeasonDataService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetStartSeasonById([FromRoute] int id)
        {
            StartSeasonDTO? startSeason = await StartSeasonDataService.GetByIdAsDTOAsync(id);
            if (startSeason != null)
            {
                return Ok(startSeason);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStartSeasons()
        {
            List<StartSeasonDTO>? startSeasons = await StartSeasonDataService.GetAllAsDTOsAsync();
            return Ok(startSeasons);
        }

        [HttpPost]
        public async Task<IActionResult> AddStartSeason(StartSeason startSeason)
        {
            StartSeasonDTO? startSeasonDTO = await StartSeasonDataService.AddAndReturnDTOAsync(startSeason);
            return Ok(startSeasonDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStartSeason(StartSeason startSeason)
        {
            StartSeasonDTO? startSeasonDTO = await StartSeasonDataService.UpdateAndReturnDTOAsync(startSeason);
            return Ok(startSeasonDTO);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteStartSeason([FromRoute] int id)
        {
            bool success = await StartSeasonDataService.DeleteAsync(id);

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
