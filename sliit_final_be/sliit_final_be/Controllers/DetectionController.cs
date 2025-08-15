using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class DetectionController : ControllerBase
{
    private readonly RoboflowService _roboflowService;

    public DetectionController(RoboflowService roboflowService)
    {
        _roboflowService = roboflowService;
    }

    // DTO for file upload
    public class DetectRequest
    {
        [Required]
        public IFormFile Image { get; set; }
    }

    /// <summary>
    /// Detect objects in an uploaded image using Roboflow.
    /// </summary>
    [HttpPost("detect")]
    [Consumes("multipart/form-data")] // Important for Swagger
    public async Task<IActionResult> Detect([FromForm] DetectRequest request)
    {
        if (request.Image == null || request.Image.Length == 0)
            return BadRequest("No image provided.");

        try
        {
            var result = await _roboflowService.DetectAsync(request.Image);
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            // Handle errors returned by Roboflow API
            return StatusCode(StatusCodes.Status502BadGateway, new
            {
                Message = "Error communicating with Roboflow API",
                Details = ex.Message
            });
        }
        catch (Exception ex)
        {
            // Generic server error
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = "An unexpected error occurred",
                Details = ex.Message
            });
        }
    }
}
