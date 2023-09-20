using System.ComponentModel.DataAnnotations;

namespace project_memu_api_server.Models
{
    public class User
    {
        [Key]
        public string user_id { get; set; }

        [Required]
        public string user_name { get; set; }

        [Required]
        public string password { get; set; }
    }
}
