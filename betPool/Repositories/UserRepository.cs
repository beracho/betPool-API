using budgetManager.Data;
using budgetManager.Models;
using budgetManager.Repositories.Interfaces;

namespace budgetManager.Repositories
{
    public class UserRepository : UserInterface
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
