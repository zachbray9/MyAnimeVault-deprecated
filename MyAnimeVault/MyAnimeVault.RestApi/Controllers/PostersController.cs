using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.RestApi.Models.DTOs;
using MyAnimeVault.RestApi.Services;

namespace MyAnimeVault.RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostersController : Controller
    {
        private readonly IPosterDataService PosterDataService;

        public PostersController(IPosterDataService posterDataService)
        {
            PosterDataService = posterDataService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetPosterById([FromRoute] int id)
        {
            PosterDTO? poster = await PosterDataService.GetByIdAsDTOAsync(id);
            if (poster != null)
            {
                return Ok(poster);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosters()
        {
            List<PosterDTO>? posters = await PosterDataService.GetAllAsDTOsAsync();
            return Ok(posters);
        }

        [HttpPost]
        public async Task<IActionResult> AddPoster(Poster poster)
        {
            PosterDTO? posterDTO = await PosterDataService.AddAndReturnDTOAsync(poster);
            return Ok(posterDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePoster(Poster poster)
        {
            PosterDTO? posterDTO = await PosterDataService.UpdateAndReturnDTOAsync(poster);
            return Ok(posterDTO);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeletePoster([FromRoute] int id)
        {
            bool success = await PosterDataService.DeleteAsync(id);

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
