namespace FinalProject_Bollean.Models.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
    }
}
