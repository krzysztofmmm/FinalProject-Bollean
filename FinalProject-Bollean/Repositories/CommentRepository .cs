using FinalProject_Bollean.Data;
using FinalProject_Bollean.Models;
using FinalProject_Bollean.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_Bollean.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly FinalProjectContext _context;

        public CommentRepository(FinalProjectContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }


        public async Task<IEnumerable<Comment>> GetCommentsForPostIdAsync(int postId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            var existingComment = await _context.Comments.FindAsync(comment.Id);
            if(existingComment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {comment.Id} not found.");
            }

            existingComment.Content = comment.Content;
            existingComment.UpdatedAt = DateTime.UtcNow;

            _context.Comments.Update(existingComment);
            await _context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if(comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
