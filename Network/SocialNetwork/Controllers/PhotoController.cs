using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private SocialNetworkDbContext context;

        public PhotoController(SocialNetworkDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            var photos = await this.context.Photos.Include(p => p.User).ToListAsync();
            return Ok(photos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "Invalid file" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var photoUrl = await SaveFile(file); // method to save the file and return its URL

            var photo = new Photo
            {
                Url = photoUrl,
                UserId = userId,
                DateAdded = DateTime.Now
            };

            this.context.Photos.Add(photo);
            await this.context.SaveChangesAsync();

            return Ok(photo);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{fileName}";
        }
    }
}
