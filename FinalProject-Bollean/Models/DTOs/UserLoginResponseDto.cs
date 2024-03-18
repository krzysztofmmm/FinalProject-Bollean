using FinalProject_Bollean.Models.Enums;

namespace FinalProject_Bollean.Models.DTOs
{
    public class UserLoginResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; } // Include optional fields
        public UserRole Role
        {
            get; set;
        }
    }
}