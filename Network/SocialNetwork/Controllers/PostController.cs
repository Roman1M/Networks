using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;
using System.Security.Claims;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private SocialNetworkDbContext context;

        public PostController(SocialNetworkDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CetPosts()
        {
            var posts = await this.context.Posts.Include(p => p.User).ToListAsync();
            return Ok(posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] PostModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Text = model.Text,
                    DatePublish = DateTime.Now,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                this.context.Posts.Add(post);
                await context.SaveChangesAsync();
                return Ok(post);
            }
            return BadRequest(ModelState);
        }
    }
}
