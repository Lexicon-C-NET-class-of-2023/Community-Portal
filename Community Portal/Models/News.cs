using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Community_Portal.Models
{
    public class News
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int UserId { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DefaultValue("News Title")]
        public string? Title { get; set; }

        [Column(TypeName = "varchar(2000)")]
        [DefaultValue("Text content")]
        public string? Content { get; set; }
    }
}