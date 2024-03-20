using FinalProject_Bollean.Data;
using FinalProject_Bollean.Models;
using FinalProject_Bollean.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_Bollean.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly FinalProjectContext _context;

        public LikeRepository(FinalProjectContext context)
        {
            _context = context;
        }

        public async Task ToggleLikeAsync(int userId , int postId , int? commentId)
        {
            if(commentId <= 0)
            {
                commentId = null;
            }

            // If commentId is specified, verify it belongs to the post
            if(commentId.HasValue)
            {
                var commentExists = await _context.Comments
                    .AnyAsync(c => c.Id == commentId.Value && c.PostId == postId);

                if(!commentExists)
                {
                    throw new ArgumentException("Comment does not exist for the given post.");
                }
            }

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId && l.CommentId == commentId);

            if(existingLike != null)
            {
                _context.Likes.Remove(existingLike);
            }
            else
            {
                _context.Likes.Add(new Like { UserId = userId , PostId = postId , CommentId = commentId });
            }

            await _context.SaveChangesAsync();
        }


        public async Task<int> CountLikesAsync(int? postId , int? commentId)
        {
            return await _context.Likes.CountAsync(l => l.PostId == postId && l.CommentId == commentId);
        }

        public async Task<int> GetLikesForPostAsync(int postId)
        {
            return await _context.Likes.CountAsync(l => l.PostId == postId);
        }

        public async Task<int> GetLikesForCommentAsync(int commentId)
        {
            return await _context.Likes.CountAsync(l => l.CommentId == commentId);
        }
    }
}
