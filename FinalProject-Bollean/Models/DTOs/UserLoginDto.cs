using System.ComponentModel.DataAnnotations;

namespace FinalProject_Bollean.Models.DTOs
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
