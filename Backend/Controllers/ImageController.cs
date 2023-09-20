using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_memu_api_server.Models;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase
{
    private readonly AppDbContext _context;
    private ILogger _logger;

    public ImageController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult UploadImage([FromForm] ImageUploadModel model)
    {
        if (model.Image == null || model.Image.Length <= 0)
        {
            return BadRequest("Image not provided.");
        }

        // Process the uploaded image
        var imagePath = ProcessUploadedFile(model.Image);

        // Save the image path and related information to the database
        var image = new Image
        {
            image_path = imagePath,
            user_id = model.UserId,
            time_stamp = DateTime.Now
        };
        if(!string.IsNullOrEmpty(model.DescriptionImage)) {
            image.description = model.DescriptionImage;
        }
        try {
            _context.Images.Add(image);
            _context.SaveChanges();

            return Ok("Image uploaded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            return StatusCode(500, "An error occurred while processing your request. Error details: " + ex.Message);
        }

    }

    [HttpGet("{image_id}/image")]
    public IActionResult GetImage(string image_id)
    {
        var image = _context.Images.FirstOrDefault(i => i.image_id == image_id);

        if (image == null)
        {
            return NotFound("Image not found.");
        }

        var imagePath = image.image_path;

        // Determine the content type based on the file extension
        var contentType = GetContentTypeFromExtension(Path.GetExtension(imagePath));

        // Serve the image using a FileResult
        return PhysicalFile(imagePath, contentType);
    }

    [HttpGet("all")]
    public IActionResult GetAllImages()
    {
        var images = _context.Images.ToList();

        if (images.Count == 0)
        {
            return NotFound("No images found.");
        }

        return Ok(images);
    }


    private string GetContentTypeFromExtension(string extension)
    {
        switch (extension.ToLower())
        {
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".png":
                return "image/png";
            case ".gif":
                return "image/gif";
            // Add more cases for other image formats as needed
            default:
                return "application/octet-stream"; // Default content type
        }
    }



    private string ProcessUploadedFile(IFormFile file)
    {
        var currentDirectory = Directory.GetCurrentDirectory(); // Get the current directory

        // Generate a unique file name
        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // Construct the directory path for images
        var imagesDirectory = Path.Combine(currentDirectory, "images"); // Adjust the path structure as needed

        // Create the directory if it doesn't exist
        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
        }

        // Construct the full path using the images directory and the unique file name
        var fullPath = Path.Combine(imagesDirectory, uniqueFileName);

        // Save the file to the full path
        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        return fullPath; // Return the full path to the saved image
    }

}
