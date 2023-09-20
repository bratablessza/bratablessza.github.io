namespace project_memu_api_server.Models
{
    public class ImageUploadModel
    {
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
        public string? DescriptionImage { get; set; } //unrequired
    }
}
