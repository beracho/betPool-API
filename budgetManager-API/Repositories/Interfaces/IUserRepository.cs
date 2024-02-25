using budgetManager.Models;

namespace budgetManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        Task<bool> EmailExists(string email);
        string GenerateRecoveryKey();
        Task<User> GetUserByUsernameOrEmail(string usernameOrEmail);
        Task<User> GetUserById(string userId);
        Task<User> UpdateUser(User user);
        Task<bool> UsernameExists(string username);
    }
}
