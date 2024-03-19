using FinalProject_Bollean.Models;

namespace FinalProject_Bollean.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> AddCommentAsync(Comment comment);
        Task<Comment> GetCommentByIdAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsForPostIdAsync(int postId);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
