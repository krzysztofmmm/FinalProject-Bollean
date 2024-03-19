using FinalProject_Bollean.Models;
using FinalProject_Bollean.Models.DTOs;
using FinalProject_Bollean.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FinalProject_Bollean.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message)> RegisterUserAsync(string email , string firstName , string lastName , string password)
        {
            if(!new EmailAddressAttribute().IsValid(email))
            {
                return (false, "Invalid email format.");
            }

            if(await _userRepository.GetUserByEmailAsync(email) != null)
            {
                return (false, "User already exists.");
            }

            if(!PasswordMeetsCriteria(password))
            {
                return (false, "Password must contain at least one uppercase letter and one number.");
            }

            var user = new User
            {
                Email = email ,
                FirstName = firstName ,
                LastName = lastName ,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _userRepository.AddUserAsync(user);
            return (true, "User registered successfully.");
        }

        private bool PasswordMeetsCriteria(string password)
        {
            return password.Any(char.IsUpper) && password.Any(char.IsDigit);
        }

        public async Task<(bool Success, UserLoginResponseDto User, string Message)> ValidateUserAsync(string email , string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null || !BCrypt.Net.BCrypt.Verify(password , user.PasswordHash))
            {
                return (false, null, "Invalid login attempt.");
            }

            var userDto = new UserLoginResponseDto
            {
                Id = user.Id ,
                Email = user.Email ,
                FirstName = user.FirstName ,
                LastName = user.LastName ,
                Bio = user.Bio ,
                Role = user.Role ,
            };
            return (true, userDto, "Login successful.");
        }

        public async Task<(bool Success, UserResponseDto User, string Message)> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if(user == null)
            {
                return (false, null, $"User with ID {id} not found.");
            }

            var userDto = ToUserResponseDto(user);
            return (true, userDto, "User found.");
        }

        public UserResponseDto ToUserResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id ,
                Email = user.Email ,
                FirstName = user.FirstName ,
                LastName = user.LastName ,
                Bio = user.Bio ,
            };
        }

        public async Task<(bool Success, User UpdatedUser, string Message)> UpdateUserAsync(int id , UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if(user == null)
            {
                return (false, null, $"User with ID {id} not found.");
            }

            user.FirstName = userUpdateDto.FirstName ?? user.FirstName;
            user.LastName = userUpdateDto.LastName ?? user.LastName;
            user.Bio = userUpdateDto.Bio ?? user.Bio;

            await _userRepository.UpdateUserAsync(user);
            return (true, user, "User updated successfully.");
        }

    }
}
