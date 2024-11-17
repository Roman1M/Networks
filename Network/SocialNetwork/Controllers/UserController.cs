using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;
using System.Security.Claims;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> userManager;

        public UserController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }
            var profile = new
            {
                user.Email,
                user.Birthdate,
                Posts = user.Posts,
                Comments = user.Comments,
                Likes = user.Likes
            };
            return Ok(profile);
        }

        [HttpPost("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            if (ModelState.IsValid)
            {
                user.Birthdate = model.Birthdate;
                var result = await this.userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { result = "Profile updated" });
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }
    }
}
