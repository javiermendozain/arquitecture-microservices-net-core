using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Users.Models;
using Api.Users.Interfaces;

namespace Api.Users.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            this._usersService = usersService;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var results = await _usersService.GetAllAsync(page, pageSize);
           
            if (results.IsSuccess) return Ok(results);
            
            return NotFound(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int? id)
        {
            if (id == null) return BadRequest();

            var result = await _usersService.GetByIdAsync((int) id);

            if (result.IsSuccess) return Ok(result);

            return NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserModel user)
        {
            if (user == null) return BadRequest();

            var result = await _usersService.AddAsync(user);
            if (result.IsSuccess) return Ok(result);

            return NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UserModel user)
        {
            var result = await _usersService.UpdateAsync(id, user);
            if (result.IsSuccess) return Ok(result);

            return NotFound(result);
        }
     
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAsync(int? userId)
        {
            if ( userId == null ) return BadRequest();

            var result = await _usersService.DeleteAsync((int) userId);
            if (result.IsSuccess) return Ok(result);

            return NotFound(result);
        }

    }
}