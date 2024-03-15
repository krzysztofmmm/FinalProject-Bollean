using FinalProject_Bollean.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject_Bollean.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string PasswordHash { get; set; }

        [Required]
        [Column("firstName")]
        public string FirstName { get; set; }

        [Column("lastName")]
        public string? LastName { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("role")]
        public UserRole Role { get; set; } = UserRole.User; // Default role, adjust as necessary

        public virtual ICollection<Post>? Posts { get; set; }
    }

}
