using budgetManager.Models;

namespace budgetManager.Repositories.Interfaces
{
    public interface UserInterface
    {
        Task<User> CreateUser(User user);
    }
}
