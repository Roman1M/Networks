using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using SocialNetwork.Models;
using Data.Entities;
using Data;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private SocialNetworkDbContext context;
        
        public CommentController(SocialNetworkDbContext context)
        {
            this.context = context;
        }

        private bool CommentExists(int id)
        {
            return this.context.Comments.Any(e => e.Id == id);
        }

        // GET: api/Comment
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await this.context.Comments.Include(c => c.User).ToListAsync();
            return Ok(comments);
        }

        // GET: api/Comment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await this.context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return NotFound(new { error = "Comment not found" });
            }
            return Ok(comment);
        }

        // POST: api/Comment
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CommentModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Text = model.Text,
                    DatePublish = DateTime.Now,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                this.context.Comments.Add(comment);
                await this.context.SaveChangesAsync();
                return Ok(comment);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Comment/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentModel model)
        {
            var comment = await this.context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound(new { error = "Comment not found" });
            }

            if (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized(new { error = "Unauthorized to edit this comment" });
            }

            comment.Text = model.Text;
            this.context.Entry(comment).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound(new { error = "Comment not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Comment/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await this.context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound(new { error = "Comment not found" });
            }

            if (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized(new { error = "Unauthorized to delete this comment" });
            }

            this.context.Comments.Remove(comment);
            await this.context.SaveChangesAsync();

            return Ok(new { result = "Comment deleted" });
        }
    }
}
