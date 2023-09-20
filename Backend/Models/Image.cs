using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_memu_api_server.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //generated id
        public string image_id { get; set; }
        [Required]
        public string image_path { get; set; }
        [Required]
        public string user_id { get; set; }

        [DataType(DataType.Text)] //un required
        public string? description { get; set; }

        [Required]
        private DateTime mTime_stamp;

        public DateTime time_stamp
        {
            get { return  mTime_stamp; }
            set {  mTime_stamp = value; }
        }

    }
}
