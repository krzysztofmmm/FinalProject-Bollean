using FinalProject_Bollean.Data;
using FinalProject_Bollean.Models;
using FinalProject_Bollean.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_Bollean.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly FinalProjectContext _context;

        public PostRepository(FinalProjectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync() => await _context.Posts.ToListAsync();

        public async Task<Post?> GetPostByIdAsync(int postId) => await _context.Posts.FindAsync(postId);

        public async Task<Post> AddPostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if(post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Post>> GetPostsByUserIdAsync(int userId)
        {
            return await _context.Posts
                                 .Where(post => post.UserId == userId)
                                 .ToListAsync();
        }
    }
}

