using budgetManager.Models;

namespace budgetManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<bool> EmailExists(string email);
        Task<User> GetUserByUsernameOrEmail(string usernameOrEmail);
        Task<User> GetUserById(string userId);
        Task<User> UpdateUser(User user);
        Task<bool> UsernameExists(string username);
    }
}
