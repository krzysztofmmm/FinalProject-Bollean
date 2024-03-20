namespace FinalProject_Bollean.Repositories.Interfaces
{
    public interface ILikeRepository
    {
        Task ToggleLikeAsync(int userId , int postId , int? commentId);
        Task<int> CountLikesAsync(int? postId , int? commentId);
        Task<int> GetLikesForPostAsync(int postId);
        Task<int> GetLikesForCommentAsync(int commentId);
        Task<bool> HasUserLikedAsync(int userId , int? postId , int? commentId);
    }
}
