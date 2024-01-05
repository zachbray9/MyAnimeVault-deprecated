using Microsoft.AspNetCore.Mvc;
using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Models.DTOs;
using MyAnimeVault.EntityFramework.Services;

namespace MyAnimeVault.RestApi.Controllers
{
    [ApiController]
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
        public async Task<IActionResult> AddUser(UserDTO newUser)
        {
            UserDTO? userDTO = await UserDataService.AddAndReturnDTOAsync(newUser);
            return Ok(userDTO);
        }

        [HttpPost]
        [Route("[action]/{userId}:int")]
        public async Task<IActionResult> AddAnimeToList([FromRoute] int userId, UserAnimeDTO anime)
        {
            bool success = await UserDataService.AddAnimeToListAsync(userId, anime);
            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {
            UserDTO? userDTO = await UserDataService.UpdateAndReturnDTOAsync(user);
            if (userDTO != null)
            {
                return Ok(userDTO);
            }

            return BadRequest();
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
        [Route("[action]/{userId:int}")]
        public async Task<IActionResult> RemoveAnimeFromList([FromRoute] int userId, UserAnimeDTO anime)
        {
            bool success = await UserDataService.RemoveAnimeFromListAsync(userId, anime);
            if(success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
