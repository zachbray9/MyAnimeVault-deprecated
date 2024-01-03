using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.EntityFramework.Services;
using MyAnimeVault.RestApi.Models.DTOs;

namespace MyAnimeVault.RestApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly MyAnimeVault.RestApi.Services.IUserDataService UserDataService;
        public UsersController(MyAnimeVault.RestApi.Services.IUserDataService userDataService)
        {
            UserDataService = userDataService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            UserDTO? user = await UserDataService.GetByIdAsDTOAsync(id);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }


        [HttpGet]
        [Route("{uid}")]
        public async Task<IActionResult> GetUserByUid([FromRoute] string uid)
        {
            UserDTO? user = await UserDataService.GetByUidAsDTOAsync(uid);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<UserDTO>? Users = await UserDataService.GetAllAsDTOsAsync();
            return Ok(Users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            UserDTO userDTO = await UserDataService.AddAndReturnDTOAsync(user);
            return Ok(userDTO);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddAnimeToList(User user, UserAnime anime)
        {
            bool success = await UserDataService.AddAnimeToListAsync(user, anime);
            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            UserDTO userDTO = await UserDataService.UpdateAndReturnDTOAsync(user);
            return Ok(userDTO);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            bool success = await UserDataService.DeleteAsync(id);

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAnimeFromList(User user, UserAnime anime)
        {
            bool success = await UserDataService.RemoveAnimeFromListAsync(user, anime);
            if(success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
