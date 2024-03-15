using System.ComponentModel.DataAnnotations;

namespace FinalProject_Bollean.Models.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100 , MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
