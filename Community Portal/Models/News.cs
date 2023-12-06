using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Community_Portal.Models
{
    public class News
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [DefaultValue("News Title")]
        public string? Title { get; set; }
        public List<NewsPost> NewsPosts { get; set; } = new List<NewsPost>();
    }
}