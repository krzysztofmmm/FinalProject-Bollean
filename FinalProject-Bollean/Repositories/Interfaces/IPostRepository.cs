using FinalProject_Bollean.Models;

namespace FinalProject_Bollean.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(int postId);
        Task<Post> AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int postId);
        Task<List<Post>> GetPostsByUserIdAsync(int userId);

    }
}
