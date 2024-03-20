using FinalProject_Bollean.Models.DTOs;
using FinalProject_Bollean.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_Bollean.Endpoints
{
    public static class LikeEndpoints
    {
        public static void ConfigureLikeEndpoints(this WebApplication app)
        {
            var likeGroup = app.MapGroup("/likes");

            likeGroup.MapPost("/toggle" , ToggleLike)
                .WithName("ToggleLike")
                .Produces(200);

            likeGroup.MapGet("/count" , CountLikes)
                .WithName("CountLikes")
                .Produces<int>(200);

            likeGroup.MapGet("/post/{postId:int}" , GetLikesForPost)
                .WithName("GetLikesForPost")
                .Produces<int>(200);

            likeGroup.MapGet("/comment/{commentId:int}" , GetLikesForComment)
                .WithName("GetLikesForComment")
                .Produces<int>(200);

            likeGroup.MapGet("/hasUserLiked" , HasUserLiked)
                      .WithName("HasUserLiked")
                      .Produces<bool>(200);
        }

        private static async Task<IResult> ToggleLike([FromBody] ToggleLikeRequestDto request , [FromServices] ILikeRepository likeRepository)
        {
            // postId is always present, so no need to check for null postId
            try
            {
                await likeRepository.ToggleLikeAsync(request.UserId , request.PostId , request.CommentId);
                return Results.Ok("Like toggled successfully.");
            }
            catch(ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> CountLikes([FromQuery] int? postId , [FromQuery] int? commentId , [FromServices] ILikeRepository likeRepository)
        {
            var count = await likeRepository.CountLikesAsync(postId , commentId);
            return Results.Ok(count);
        }

        private static async Task<IResult> GetLikesForPost(int postId , [FromServices] ILikeRepository likeRepository)
        {
            var likeCount = await likeRepository.GetLikesForPostAsync(postId);
            return Results.Ok(new { PostId = postId , LikeCount = likeCount });
        }

        private static async Task<IResult> GetLikesForComment(int commentId , [FromServices] ILikeRepository likeRepository)
        {
            var likeCount = await likeRepository.GetLikesForCommentAsync(commentId);
            return Results.Ok(new { CommentId = commentId , LikeCount = likeCount });
        }

        private static async Task<IResult> HasUserLiked([FromQuery] int userId , [FromQuery] int? postId , [FromQuery] int? commentId , [FromServices] ILikeRepository likeRepository)
        {
            var hasLiked = await likeRepository.HasUserLikedAsync(userId , postId , commentId);
            return Results.Ok(hasLiked);
        }
    }
}
