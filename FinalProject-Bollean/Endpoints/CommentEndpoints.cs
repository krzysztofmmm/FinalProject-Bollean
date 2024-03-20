using FinalProject_Bollean.Models;
using FinalProject_Bollean.Models.DTOs;
using FinalProject_Bollean.Repositories.Interfaces;

namespace FinalProject_Bollean.Endpoints
{
    public static class CommentEndpoints
    {
        public static void ConfigureCommentEndpoints(this WebApplication app)
        {
            var commentGroup = app.MapGroup("/comments");

            commentGroup.MapPost("/" , CreateComment)
                .WithName("CreateComment")
                .Accepts<CommentCreateDto>("application/json")
                .Produces<CommentResponseDto>(201);

            commentGroup.MapGet("/post/{postId:int}" , GetCommentsForPost)
                .WithName("GetCommentsByPost")
                .Produces<IEnumerable<CommentResponseDto>>(200)
                .Produces(404);

            commentGroup.MapPut("/{id:int}" , UpdateComment)
                .WithName("UpdateComment")
                .Accepts<CommentUpdateDto>("application/json")
                .Produces<CommentResponseDto>(200)
                .Produces(404);

            commentGroup.MapDelete("/{id:int}" , DeleteComment)
                .WithName("DeleteComment")
                .Produces(204)
                .Produces(404);
        }

        private static async Task<IResult> CreateComment(CommentCreateDto dto , ICommentRepository repository)
        {
            if(string.IsNullOrWhiteSpace(dto.Content))
            {
                return Results.BadRequest("Content cannot be empty.");
            }

            var comment = new Comment
            {
                UserId = dto.UserId ,
                PostId = dto.PostId ,
                Content = dto.Content ,
                CreatedAt = DateTime.UtcNow ,
                UpdatedAt = DateTime.UtcNow ,
            };

            var createdComment = await repository.AddCommentAsync(comment);
            return Results.Created($"/comments/{createdComment.Id}" , createdComment.AsDto());
        }

        public static async Task<IResult> GetCommentsForPost(int postId , ICommentRepository repository)
        {
            var comments = await repository.GetCommentsForPostIdAsync(postId);

            var commentDtos = comments.Select(c => new CommentResponseDto
            {
                Id = c.Id ,
                UserId = c.UserId ,
                UserName = c.User != null ? $"{c.User.FirstName} {c.User.LastName}" : "Unknown" ,
                PostId = c.PostId ,
                Content = c.Content ,
                CreatedAt = c.CreatedAt ,
                UpdatedAt = c.UpdatedAt
            });

            if(!commentDtos.Any())
            {
                return Results.NotFound($"No comments found for post with ID {postId}.");
            }

            return Results.Ok(commentDtos);
        }

        private static async Task<IResult> UpdateComment(int id , CommentUpdateDto dto , ICommentRepository repository)
        {
            var existingComment = await repository.GetCommentByIdAsync(id);
            if(existingComment == null)
            {
                return Results.NotFound($"Comment with ID {id} not found.");
            }

            if(string.IsNullOrWhiteSpace(dto.Content))
            {
                return Results.BadRequest("Content cannot be empty.");
            }

            existingComment.Content = dto.Content;
            existingComment.UpdatedAt = DateTime.UtcNow;

            await repository.UpdateCommentAsync(existingComment);
            return Results.Ok(existingComment.AsDto());
        }

        private static async Task<IResult> DeleteComment(int id , ICommentRepository repository)
        {
            var deleted = await repository.DeleteCommentAsync(id);
            if(!deleted)
            {
                return Results.NotFound("Comment not found");
            }

            return Results.NoContent();
        }

        private static CommentResponseDto AsDto(this Comment comment)
        {
            var userName = comment.User != null ? $"{comment.User.FirstName} {comment.User.LastName}" : "Unknown";


            return new CommentResponseDto
            {
                Id = comment.Id ,
                UserId = comment.UserId ,
                UserName = userName ,
                PostId = comment.PostId ,
                Content = comment.Content ,
                CreatedAt = comment.CreatedAt ,
                UpdatedAt = comment.UpdatedAt
            };
        }
    }
}
