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
    public class MessageController : ControllerBase
    {
        private SocialNetworkDbContext context;
        public MessageController(SocialNetworkDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMessages()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = await this.context.Messages
                .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToListAsync();
            return Ok(messages);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] MessageModel model)
        {
            if (ModelState.IsValid)
            {
                var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var message = new Message
                {
                    SenderId = senderId,
                    ReceiverId = model.ReceiverId,
                    Content = model.Content,
                    Timestamp = DateTime.Now
                };

                this.context.Messages.Add(message);
                await this.context.SaveChangesAsync();

                return Ok(message);
            }
            return BadRequest(ModelState);
        }
    }
}
