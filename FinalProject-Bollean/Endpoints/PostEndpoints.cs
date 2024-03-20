using FinalProject_Bollean.Models;
using FinalProject_Bollean.Models.DTOs;
using FinalProject_Bollean.Repositories.Interfaces;

namespace FinalProject_Bollean.Endpoints
{
    public static class PostEndpoints
    {
        public static void ConfigurePostEndpoints(this WebApplication app)
        {
            var postGroup = app.MapGroup("/posts");

            postGroup.MapPost("/" , CreatePost)
                .WithName("CreatePost")
                .Accepts<PostCreateDto>("application/json")
                .Produces<PostResponseDto>(201);

            postGroup.MapGet("/" , GetAllPosts)
                .WithName("GetAllPosts")
                .Produces<PostResponseDto[]>(200);

            postGroup.MapGet("/{id:int}" , GetPostById)
                .WithName("GetPostById")
                .Produces<PostResponseDto>(200)
                .Produces(404);

            postGroup.MapPut("/{id:int}" , UpdatePost)
                .WithName("UpdatePost")
                .Accepts<PostUpdateDto>("application/json")
                .Produces<PostResponseDto>(200)
                .Produces(404);

            postGroup.MapDelete("/{id:int}" , DeletePost)
                .WithName("DeletePost")
                .Produces(204)
                .Produces(404);

            postGroup.MapGet("/user/{userId:int}" , GetPostsByUser)
                .WithName("GetPostsByUser")
                .Produces<PostResponseDto[]>(200)
                .Produces(404);
        }

        private static async Task<IResult> CreatePost(PostCreateDto dto , IPostRepository repository)
        {
            if(string.IsNullOrWhiteSpace(dto.Content))
            {
                return Results.BadRequest("Content cannot be empty.");
            }

            var post = new Post
            {
                UserId = dto.UserId ,
                Title = dto.Title ?? "Title" ,
                Tag = dto.Tag ,
                Content = dto.Content ,
                CreatedAt = DateTime.UtcNow ,
                UpdatedAt = DateTime.UtcNow ,
            };

            var createdPost = await repository.AddPostAsync(post);
            return Results.Created($"/posts/{createdPost.Id}" , createdPost.AsDto());
        }


        private static async Task<IResult> GetAllPosts(IPostRepository repository)
        {
            var posts = await repository.GetAllPostsAsync();
            return Results.Ok(posts.Select(p => p.AsDto()));
        }

        private static async Task<IResult> GetPostById(int id , IPostRepository repository)
        {
            var post = await repository.GetPostByIdAsync(id);
            if(post == null) return Results.NotFound();

            return Results.Ok(post.AsDto());
        }

        private static async Task<IResult> UpdatePost(int id , PostUpdateDto dto , IPostRepository repository)
        {
            var post = await repository.GetPostByIdAsync(id);
            if(post == null) return Results.NotFound();

            post.Title = dto.Title ?? post.Title;

            if(!string.IsNullOrWhiteSpace(dto.Content))
            {
                post.Content = dto.Content;
            }
            else
            {
                return Results.BadRequest("Content cannot be empty.");
            }

            post.Tag = dto.Tag ?? post.Tag;
            post.UpdatedAt = DateTime.UtcNow;

            await repository.UpdatePostAsync(post);
            return Results.Ok(post.AsDto());
        }



        private static async Task<IResult> DeletePost(int id , IPostRepository repository)
        {
            var post = await repository.GetPostByIdAsync(id);
            if(post == null) return Results.NotFound();

            await repository.DeletePostAsync(id);

            return Results.NoContent();
        }

        private static async Task<IResult> GetPostsByUser(int userId , IPostRepository repository)
        {
            var posts = await repository.GetPostsByUserIdAsync(userId);
            if(!posts.Any())
            {
                return Results.NotFound($"No posts found for user with ID {userId}.");
            }
            return Results.Ok(posts.Select(post => post.AsDto()));
        }

        private static PostResponseDto AsDto(this Post post) => new PostResponseDto
        {
            Id = post.Id ,
            UserId = post.UserId ,
            Title = post.Title ,
            Tag = post.Tag ,
            Content = post.Content ,
            CreatedAt = post.CreatedAt ,
            UpdatedAt = post.UpdatedAt
        };
    }
}
