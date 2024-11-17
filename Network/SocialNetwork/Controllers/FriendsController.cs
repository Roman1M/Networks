using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private SocialNetworkDbContext context;
        public FriendsController(SocialNetworkDbContext context)
        {
            this.context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFriend(string userId, string friendId)
        {
            if (userId == friendId)
                return BadRequest("User cannot add themselves as a friend.");

            var user = await this.context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId);
            var friend = await this.context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == friendId);

            if (user == null || friend == null)
                return NotFound("User not found.");

            if (user.Friends.Contains(friend))
                return BadRequest("Already friends.");

            user.Friends.Add(friend);
            await this.context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFriend(string userId, string friendId)
        {
            var user = await this.context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId);
            var friend = await this.context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == friendId);

            if (user == null || friend == null)
                return NotFound("User not found.");

            if (!user.Friends.Contains(friend))
                return BadRequest("Not friends.");

            user.Friends.Remove(friend);
            await this.context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("list/{userId}")]
        public async Task<IActionResult> GetFriends(string userId)
        {
            var user = await this.context.Users
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user.Friends);
        }
    }
}
