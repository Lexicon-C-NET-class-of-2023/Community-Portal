using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Community_Portal.Models
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public int ForumId { get; set; }
        [JsonIgnore]
        public Forum Forum { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        [DefaultValue("Text content")]
        public string Content { get; set; }
    }
}