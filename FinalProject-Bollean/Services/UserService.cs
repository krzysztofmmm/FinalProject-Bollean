﻿using FinalProject_Bollean.Models;
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

        public async Task<(bool Success, string Message)> RegisterUserAsync(string email , string firstName , string password)
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _userRepository.AddUserAsync(user);
            return (true, "User registered successfully.");
        }

        private bool PasswordMeetsCriteria(string password)
        {
            return password.Any(char.IsUpper) && password.Any(char.IsDigit);
        }

        public async Task<(bool Success, string Message)> ValidateUserAsync(string email , string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user == null || !BCrypt.Net.BCrypt.Verify(password , user.PasswordHash))
            {
                return (false, "Invalid login attempt.");
            }

            return (true, "Login successful.");
        }
    }
}