using budgetManager.Models;

namespace budgetManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<bool> UsernameExists(string username);
        Task<bool> EmailExists(string email);
        Task<User> GetUserById(string userId);
    }
}
