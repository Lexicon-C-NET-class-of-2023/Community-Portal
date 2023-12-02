using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Community_Portal.Models
{
    public class Forum
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [DefaultValue("Forum Title")]
        public string? Title { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}