using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Imagekit.Sdk;

namespace Bookstore.Web.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class ImageUploadController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ImageUploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("image")]
        public async Task<ActionResult<ApiResponse<string>>> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "No file provided"
                });
            }

            try
            {
                var publicKey = _configuration["ImageKit:PublicKey"];
                var privateKey = _configuration["ImageKit:PrivateKey"];
                var urlEndpoint = _configuration["ImageKit:UrlEndpoint"];

                // Convert file to base64
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var base64 = Convert.ToBase64String(fileBytes);

                // Generate unique filename
                var fileName = $"book_{DateTime.UtcNow.Ticks}_{Path.GetFileNameWithoutExtension(file.FileName)}{Path.GetExtension(file.FileName)}";

                // Use ImageKit SDK with Imagekit.Sdk namespace
                var imagekit = new ImagekitClient(publicKey, privateKey, urlEndpoint);
                
                var uploadRequest = new FileCreateRequest
                {
                    file = base64,
                    fileName = fileName,
                    folder = "/books",
                    useUniqueFileName = true
                };

                var result = imagekit.Upload(uploadRequest);

                if (result != null && !string.IsNullOrEmpty(result.url))
                {
                    return Ok(new ApiResponse<string>
                    {
                        Success = true,
                        Message = "Image uploaded successfully",
                        Data = result.url
                    });
                }

                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to upload image to ImageKit"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error uploading image: {ex.Message}"
                });
            }
        }
    }
}
