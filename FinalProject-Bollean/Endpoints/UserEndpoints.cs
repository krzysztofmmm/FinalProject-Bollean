using FinalProject_Bollean.Models;
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
            app.MapGroup("/users")
                .MapPut("/users/{id:int}" , UpdateUserEndpoint)
                .WithTags("Users")
                .WithName("UpdateUser")
                .Produces<UserResponseDto>(200)
                .Produces(404);
        }

        private static async Task<IResult> RegisterUser(UserRegisterDto userRegisterDto , UserService userService)
        {
            var (success, message) = await userService.RegisterUserAsync(
                userRegisterDto.Email ,
                userRegisterDto.FirstName ,
                userRegisterDto.LastName ,
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

        private static async Task<IResult> UpdateUserEndpoint(int id , UserUpdateDto userUpdateDto , UserService userService)
        {
            var (success, updatedUser, message) = await userService.UpdateUserAsync(id , userUpdateDto);
            if(!success)
            {
                return Results.Problem(message);
            }

            var userResponseDto = AsDto(updatedUser);
            return Results.Ok(userResponseDto);
        }

        private static UserResponseDto AsDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id ,
                Email = user.Email ,
                FirstName = user.FirstName ,
                LastName = user.LastName ,
                Bio = user.Bio
            };
        }
    }
}
