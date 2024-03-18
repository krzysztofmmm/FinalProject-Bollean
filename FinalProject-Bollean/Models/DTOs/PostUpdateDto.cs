using System.ComponentModel.DataAnnotations;

namespace FinalProject_Bollean.Models.DTOs
{
    public class PostUpdateDto
    {
        public string? Title { get; set; } // Optional
        public string? Tag { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
