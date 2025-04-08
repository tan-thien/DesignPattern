using Microsoft.AspNetCore.Mvc;

namespace Swagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ImagesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                return BadRequest("Tên ảnh không hợp lệ.");
            }

            if (string.IsNullOrEmpty(_env.WebRootPath))
            {
                return BadRequest("WebRootPath is not configured.");
            }

            var imagePath = Path.Combine(_env.WebRootPath, "images", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg"); // Thay đổi định dạng nếu cần
        }
    }


}
