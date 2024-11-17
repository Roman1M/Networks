using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;
using System.Data;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        public UserManager<User> userManager;

        public AdminController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = this.userManager.Users;
            return Ok(users);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }
            var result = await this.userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { result = "User deleted" });
            }
            return BadRequest(result.Errors);
        }
    }
}
