namespace FinalProject_Bollean.Models.DTOs
{
    public class ToggleLikeRequestDto
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int? CommentId { get; set; }
    }
}
