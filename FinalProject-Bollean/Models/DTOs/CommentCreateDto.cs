namespace FinalProject_Bollean.Models.DTOs
{
    public class CommentCreateDto
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
    }

}
