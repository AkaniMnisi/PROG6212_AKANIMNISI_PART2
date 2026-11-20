using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data; 
using RaceDay.API.Models; 
using Azure.Storage.Blobs; 
using Microsoft.AspNetCore.Http; 

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        private readonly IConfiguration _configuration; 

      
        public EventsController(RaceDayContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

     

    
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
               
                var connectionString = _configuration.GetConnectionString("AzureStorage");

               
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient("event-images");

                
                await containerClient.CreateIfNotExistsAsync();

               
                var blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var blobClient = containerClient.GetBlobClient(blobName);

              
                using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, true);

               
                return Ok(new { ImageUrl = blobClient.Uri.ToString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}