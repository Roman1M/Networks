using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SocialNetwork.Models;
using Data;
using Data.Entities;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private SocialNetworkDbContext context;

        public LikeController(SocialNetworkDbContext context)
        {
            this.context = context;
        }

        [HttpPost("like")]
        [Authorize]
        public async Task<IActionResult> LikePost([FromBody] int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var like = new Like
            {
                PostId = postId,
                UserId = userId
            };
            this.context.Likes.Add(like);
            await this.context.SaveChangesAsync();
            return Ok(new { result = "Post liked" });
        }

        [HttpPost("unlike")]
        [Authorize]
        public async Task<IActionResult> UnlikePost([FromBody] int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var like = await this.context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (like != null)
            {
                this.context.Likes.Remove(like);
                await this.context.SaveChangesAsync();
                return Ok(new { result = "Post unliked" });
            }
            return NotFound(new { error = "Like not found" });
        }
    }
}
