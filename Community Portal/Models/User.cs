using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Community_Portal.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}