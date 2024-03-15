using FinalProject_Bollean.Data;
using FinalProject_Bollean.Models;
using FinalProject_Bollean.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_Bollean.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinalProjectContext _context;

        public UserRepository(FinalProjectContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
