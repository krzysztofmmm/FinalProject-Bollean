using FinalProject_Bollean.Models.DTOs;
using FinalProject_Bollean.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_Bollean.Endpoints
{
    public static class UserEndpoints
    {
        public static void ConfigureUserEndpoints(this WebApplication app)
        {
            app.MapGroup("/users")
               .MapPost("/register" , RegisterUser)
               .WithTags("Users")
               .Produces<UserRegisterDto>()
               .Produces(200)
               .Produces(400)
               .WithName("RegisterUser");

            app.MapGroup("/users")
               .MapPost("/login" , LoginUser)
               .WithTags("Users")
               .Produces<UserLoginDto>()
               .Produces(200)
               .Produces(400)
               .WithName("LoginUser");

            app.MapGroup("/users")
                .MapGet("/{id:int}" , GetUserById)
                .WithTags("Users")
                .Produces<UserResponseDto>(200)
                .Produces(404)
                .WithName("GetUserById");
        }

        private static async Task<IResult> RegisterUser(UserRegisterDto userRegisterDto , UserService userService)
        {
            var (success, message) = await userService.RegisterUserAsync(
                userRegisterDto.Email ,
                userRegisterDto.FirstName ,
                userRegisterDto.Password);

            return success ? Results.Ok(message) : Results.BadRequest(message);
        }

        private static async Task<IResult> LoginUser(UserLoginDto userLoginDto , UserService userService)
        {
            var (success, user, message) = await userService.ValidateUserAsync(userLoginDto.Email , userLoginDto.Password);
            if(!success)
            {
                return Results.BadRequest(message);
            }
            return Results.Ok(user);
        }

        private static async Task<IResult> GetUserById(int id , [FromServices] UserService userService)
        {
            var (success, userDto, message) = await userService.GetUserByIdAsync(id);
            if(!success)
            {
                return Results.NotFound(message);
            }
            return Results.Ok(userDto);
        }
    }
}
