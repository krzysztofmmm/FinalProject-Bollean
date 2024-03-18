using FinalProject_Bollean.Models;

namespace FinalProject_Bollean.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
    }
}
