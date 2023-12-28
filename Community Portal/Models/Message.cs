using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Community_Portal.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int UserId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string SenderName { get; set; }

        public int Recipient { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string RecipientName { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string Content { get; set; }
    }
}