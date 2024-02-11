using budgetManager.Data;
using budgetManager.Models;
using budgetManager.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace budgetManager.Repositories
{
    public class UserRepository : IUserRepository
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

        public async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<User> GetUserById(string userId)
        {
            Guid userGuid = new Guid(userId);
            var userFromRepo = await _context.Users.FirstOrDefaultAsync(x => x.Id == userGuid);
            return userFromRepo == null ? new User() : userFromRepo;
        }
    }
}
